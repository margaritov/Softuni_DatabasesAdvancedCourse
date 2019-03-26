namespace CustomAutoMapper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;


    public class Mapper
    {
        public T Map<T>(object source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source cannot be null!");
            }

            T dest = (T)Activator.CreateInstance(typeof(T));


            return DoMapping(source, dest);
        }

        private T DoMapping<T>(object source, T dest)
        {
            var destProperties = dest
                 .GetType()
                 .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                 .Where(p => p.CanWrite);

            var srcProperties = source
                 .GetType()
                 .GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var destProperty in destProperties)
            {
                var srcProperty = srcProperties
                    .Where(p => p.Name == destProperty.Name)
                    .FirstOrDefault();

                if (srcProperty == null)
                {
                    continue;
                }

                try
                {
                    destProperty.SetValue(dest, srcProperty.GetValue(source));
                }
                catch (Exception ex)
                {

                    throw;
                }

            }

            return dest;
        }
    }
}
