using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ORMFramework.Static
{
    public static class Helpers
    {
        //Create list 
        public static IList CreateList(Type type)
        {
            Type genericListType = typeof(List<>).MakeGenericType(type);
            return (IList)Activator.CreateInstance(genericListType);
        }
        //Convert a DataRow to Object
        public static T ToEntity<T>(this DataRow row) where T : class
        {
            IList<PropertyInfo> properties = typeof(T).GetProperties().ToList();
            T item = Activator.CreateInstance<T>();
            foreach (var property in properties)
            {
                try
                {
                    if (property.PropertyType == typeof(System.DayOfWeek))
                    {
                        DayOfWeek day = (DayOfWeek)System.Enum.Parse(typeof(DayOfWeek), row[property.Name].ToString());
                        property.SetValue(item, day, null);
                    }
                    else
                    {
                        if (row[property.Name] == DBNull.Value)
                            property.SetValue(item, null, null);
                        else
                            property.SetValue(item, row[property.Name], null);
                    }
                }
                catch (Exception ex)
                {
                    property.SetValue(item, null, null);
                }

            }
            return item;
        }
        //Convert DataReader to List
        public static List<T> DataReaderMapToList<T>(IDataReader dr)
        {
            List<T> list = new List<T>();
            T obj = default(T);
            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    try
                    {
                        if (prop.PropertyType == typeof(System.DayOfWeek))
                        {
                            DayOfWeek day = (DayOfWeek)System.Enum.Parse(typeof(DayOfWeek), dr[prop.Name].ToString());
                            prop.SetValue(obj, day, null);
                        }
                        else
                        {
                            if (dr[prop.Name] == DBNull.Value)
                                prop.SetValue(obj, null, null);
                            else
                                prop.SetValue(obj, dr[prop.Name], null);
                        }
                    }
                    catch (Exception ex)
                    {
                        prop.SetValue(obj, null, null);
                    }
                    //if (!object.Equals(dr[prop.Name], DBNull.Value))
                    //{
                    //    prop.SetValue(obj, dr[prop.Name], null);
                    //}
                }
                list.Add(obj);
            }
            return list;
        }
        // format string for SQL
        public static object format(object value)
        {
            if (value is string || value is bool || value is DateTime)
            {
                return $"'{value}'";
            }
            return value;
        }
        // Convert Expression to string conditons
        public static string GetWhereClause<T>(Expression<Func<T, bool>> expression)
        {
            return GetValueAsString(expression.Body);
        }

        public static string GetValueAsString(Expression expression)
        {
            var value = "";
            var equalty = "";
            var left = GetLeftNode(expression);
            var right = GetRightNode(expression);
            if (expression.NodeType == ExpressionType.Equal)
            {
                equalty = "=";
            }
            if (expression.NodeType == ExpressionType.AndAlso)
            {
                equalty = "AND";
            }
            if (expression.NodeType == ExpressionType.OrElse)
            {
                equalty = "OR";
            }
            if (expression.NodeType == ExpressionType.NotEqual)
            {
                equalty = "<>";
            }
            if (left is MemberExpression)
            {
                var leftMem = left as MemberExpression;
                value = string.Format("{0}{1}{2}", leftMem.Member.Name, equalty, "{0}");
            }
            if (right is ConstantExpression)
            {
                var rightConst = right as ConstantExpression;
                value = string.Format(value, format(rightConst.Value));
            }
            if (right is MemberExpression)
            {
                var rightMem = right as MemberExpression;
                var rightConst = rightMem.Expression as ConstantExpression;
                var member = rightMem.Member.DeclaringType;
                var type = rightMem.Member.MemberType;
                var val = member.GetField(rightMem.Member.Name).GetValue(rightConst.Value);
                value = string.Format(value, format(val));
            }
            if (value == "")
            {
                var leftVal = GetValueAsString(left);
                var rigthVal = GetValueAsString(right);
                value = string.Format("{0} {1} {2}", leftVal, equalty, format(rigthVal));
            }
            return value;
        }

        private static Expression GetLeftNode(Expression expression)
        {
            dynamic exp = expression;
            return ((Expression)exp.Left);
        }

        private static Expression GetRightNode(Expression expression)
        {
            dynamic exp = expression;
            return ((Expression)exp.Right);
        }
        public static object GetObject(this Dictionary<string, object> dict, Type type)
        {
            var obj = Activator.CreateInstance(type);

            foreach (var kv in dict)
            {
                var prop = type.GetProperty(kv.Key);
                if (prop == null) continue;

                object value = kv.Value;
                if (value is Dictionary<string, object>)
                {
                    value = GetObject((Dictionary<string, object>)value, prop.PropertyType); // <= This line
                }

                prop.SetValue(obj, value, null);
            }
            return obj;
        }
        //Convert Dictionary to Object
        public static T GetObject<T>(this Dictionary<string, object> dict)
        {
            return (T)GetObject(dict, typeof(T));
        }
        public static List<string> GetPropertyNames<T>()
        {

            var tableType = typeof(T);
            var result = tableType.GetProperties(BindingFlags.GetProperty
                                                 | BindingFlags.Instance
                                                 | BindingFlags.Public)
              .Select(p => p.Name)
              .ToList();

            return result;
        }

        public static object GetValue(object classInstance, string propertyName)
        {
            var tableType = classInstance.GetType();
            var prop = tableType.GetProperty(propertyName);
            var result = prop.GetValue(classInstance);

            return result;
        }
        //Convert Object to Dictionary
        public static Dictionary<string, object> ToDictionary(this object source)
        {
            return source.ToDictionary<object>();
        }

        public static Dictionary<string, T> ToDictionary<T>(this object source)
        {
            if (source == null)
                ThrowExceptionWhenSourceArgumentIsNull();

            var dictionary = new Dictionary<string, T>();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(source))
                AddPropertyToDictionary<T>(property, source, dictionary);
            return dictionary;
        }

        private static void AddPropertyToDictionary<T>(PropertyDescriptor property, object source, Dictionary<string, T> dictionary)
        {
            object value = property.GetValue(source);
            if (IsOfType<T>(value))
                dictionary.Add(property.Name, (T)value);
        }

        private static bool IsOfType<T>(object value)
        {
            return value is T;
        }

        private static void ThrowExceptionWhenSourceArgumentIsNull()
        {
            throw new ArgumentNullException("source", "Unable to convert object to a dictionary. The source object is null.");
        }
    }
}
