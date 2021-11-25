using System;
using System.Collections.Generic;
using Renga;

namespace TX_SetPositions
{
    public class GetNamesCollection
    {
        public List<string> list { get; private set; }        
        public List<IModelObject> modelObjectList { get; private set; }

        public GetNamesCollection(Guid FilterType, IModelObjectCollection modelObjectCollection)
        {         
            List<IModelObject> modelObjectList = new List<IModelObject>();            
            this.modelObjectList = modelObjectList;

            List<string> list = new List<string>();
            this.list = list;

            for (int i = 0; i < modelObjectCollection.Count; ++i)
            {

                IModelObject modelObject = modelObjectCollection.GetByIndex(i);
                if (modelObject.ObjectType == FilterType) //* && некий параметр = "ТХ" )
                {
                    modelObjectList.Add(modelObject);  // собирает все объекты в проекте, отвечающие заданному типу
                    if (!list.Contains(modelObject.Name)) // собирает имена объектов в проекте, отвечающих заданному типу, исключая софпадения
                    {

                        list.Add(modelObject.Name);
                    }
                }
            }
        }
    }
}
