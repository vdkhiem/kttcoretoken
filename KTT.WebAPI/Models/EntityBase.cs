using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KTT.WebAPI.Models
{
    public class EntityBase
    {
        public int Id { get; set; }

        /// <summary>
        /// High level clone
        /// </summary>
        /// <returns></returns>
        public EntityBase ShallowCopy()
        {
            return (EntityBase)this.MemberwiseClone();
        }

        /// <summary>
        /// Copy values from parent to child
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        public void Copy<T>(T parent)
        {
            var parentProperties = parent.GetType().GetProperties();
            var childProperties = this.GetType().GetProperties();

            foreach (var parentProperty in parentProperties)
            {
                foreach (var childProperty in childProperties)
                {
                    if (parentProperty.Name == childProperty.Name && parentProperty.PropertyType == childProperty.PropertyType)
                    {
                        childProperty.SetValue(this, parentProperty.GetValue(parent));
                        break;
                    }
                }
            }
        }
    }
}
