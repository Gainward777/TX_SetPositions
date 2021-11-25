using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Renga;

namespace TX_SetPositions
{
    public class SetPositions
    {
        public int count { get; private set; }
        public int passingCount { get; private set; }

        public SetPositions(Guid FilterType, string PropIdAsString, IModelObjectCollection modelObjectCollection)
        {
            GetNamesCollection namesCollection = new GetNamesCollection(FilterType, modelObjectCollection); // собирает объекты переданного типа и их имена в массивы типа List<>
            IEnumerator enumerator = IdNumerator(namesCollection.list, namesCollection.modelObjectList);

            int count = 1;
            int passingCount = 0;            

            while (enumerator.MoveNext())
            {                
                foreach (int id in (List<int>)enumerator.Current)
                {
                    IModelObject modelObject = modelObjectCollection.GetById(id);


                    IPropertyContainer propertyContainer = modelObject.GetProperties();

                    propertyContainer.Get(Guid.Parse(PropIdAsString)).ResetValue();
                    propertyContainer.Get(Guid.Parse(PropIdAsString)).SetStringValue(count.ToString());
                    passingCount++;
                }
                count++;
            }
            this.count = count - 1;
            this.passingCount = passingCount;            
        }

        public IEnumerator IdNumerator(List<string> list, List<IModelObject> modelObjectsList) 
        {
            for (int i = 0; i < list.Count; i++)
            {                
                List<IModelObject> objects = modelObjectsList.Where(c => list[i] == c.Name)
                                              .ToList();          
                List<int> listIds = new List<int>();
                foreach (var obj in objects)
                {

                    listIds.Add(obj.Id);
                }                
                yield return listIds;
            }
        }
    }
}
