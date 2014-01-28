using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Reflection;


namespace WMS.Configurations
{
    [ConfigurationCollection(typeof(ExternalWebServiceElement), CollectionType = ConfigurationElementCollectionType.BasicMap, AddItemName = "add")]
    public class ExternalWebServiceCollection : ConfigurationElementCollection
    {
        public void Add(ExternalWebServiceElement element)
        {
            base.BaseAdd(element);
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ExternalWebServiceElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ExternalWebServiceElement)element).Name;
        }

        public override bool IsReadOnly()
        {
            return false;
        }

        public void Remove(ExternalWebServiceElement element)
        {
            base.BaseRemove(this.GetElementKey(element));
        }

        public void Remove(string name)
        {
            base.BaseRemove(name);
        }

        public ExternalWebServiceElement this[int index]
        {
            get
            {
                return (base.BaseGet(index) as ExternalWebServiceElement);
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }
    }
}
