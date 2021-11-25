using Renga;

namespace TX_SetPositions
{
    public class GetPropertyId
    {
        public static string Name(string PropertyName)
        {
            string ID = string.Empty;
            IApplication project = new Application();

            IPropertyManager propertyManager = project.Project.PropertyManager;
            IModel model = project.Project.Model;
            IModelObjectCollection modelObjectCollection = model.GetObjects();
            IOperation operation = model.CreateOperation();
            
            for (int i = 0; i < propertyManager.PropertyCount; i++)
            {
                string prId = propertyManager.GetPropertyIdS(i);
                string name = propertyManager.GetPropertyNameS(prId);
                if (name == PropertyName)
                    ID = prId;
            }
            return ID;
        }
    }
}
