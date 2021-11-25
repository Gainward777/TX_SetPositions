using System;
using Renga;

namespace TX_SetPositions
{
    class TX_SetPositions : IPlugin
    {
        public ActionEventSource m_myActionEvents;
        public string NumbersCount = string.Empty;

        public int indexCount { get; set; }
        public int positionCount { get; set; }

        IOperation operation;

        public bool Initialize(string pluginFolder)
        {
            IApplication aplication = new Application();
            
            IUI ui = aplication.UI;
            IAction action = ui.CreateAction();

            string imagePath = pluginFolder + @"\NumberTheFamily.png";      // относительный путь к картинке в папке плагина
            IImage image = ui.CreateImage(); //работает через раз... НЕ БАГУЕТ, ЕСЛИ ЯВНО ОБЪЯВЛЯТЬ ПЕРЕМЕННУЮ ЧЕРЕЗ ИНТЕРФЕЙС!!!
            image.LoadFromFile(imagePath);   
            action.Icon = image;

            action.DisplayName = "TX_SetPositions";
            action.ToolTip = "TX_SetPositions";
               
            IUIPanelExtension panelExtension = ui.CreateUIPanelExtension();
            
            m_myActionEvents = new ActionEventSource(action); // should stay alive as long as you need to handle the events
            m_myActionEvents.Triggered += (sender, args) =>
            {
                try
                {
                    SetNumbers();

                    ui.ShowMessageBox(MessageIcon.MessageIcon_Info, "Результат", $"Позиций проставлено: {positionCount}\nВсего объектов пронумеровано {indexCount}");    /* handle the Triggered event */
                }
                catch (FormatException ex)
                {
                    ui.ShowMessageBox(MessageIcon.MessageIcon_Error, "Error", $"В проекте остутствует подходящее свойство.");

                    operation.Apply();  
                }
                catch (NullReferenceException ex)
                {
                    ui.ShowMessageBox(MessageIcon.MessageIcon_Error, "Error", $"Свойство присутствует в проекте, но не присвоено элементам.");

                    operation.Apply();                    
                }
                catch (ArgumentNullException ex)
                {
                    ui.ShowMessageBox(MessageIcon.MessageIcon_Error, "Error", $"Аргументу не присваивается значение.");

                    operation.Apply();                  
                }
                catch (Exception ex)
                {
                    ui.ShowMessageBox(MessageIcon.MessageIcon_Error, "Error", $"Формат свойства не соответствует заданному значению.\nСвойство должно иметь формат строки.");

                    operation.Apply();                   
                }                       
            };
            panelExtension.AddToolButton(action);
            ui.AddExtensionToPrimaryPanel(panelExtension);
            
            return true;
        }
        public void Stop()
        {
            m_myActionEvents.Dispose();    // срабатывает только при закрытии самой Renga?            
        }

        public void SetNumbers()
        {
            IApplication project = new Renga.Application();

            IPropertyManager propertyManager = project.Project.PropertyManager;
            IModel model = project.Project.Model;
            IModelObjectCollection modelObjectCollection = model.GetObjects();
            operation = model.CreateOperation();

            operation.Start();   

            try
            {
                string PropIdAsString = GetPropertyId.Name("ТХ_Позиция");
                PropertyType propertyType = propertyManager.GetPropertyTypeS(PropIdAsString);
                if (propertyType.ToString() != "PropertyType_String" && propertyType.ToString() != "PropertyType_Undefined") 
                {
                   throw new Exception();                    
                }

                SetPositions setPositions = new SetPositions(ObjectTypes.Element, PropIdAsString, modelObjectCollection); // проставляет позиции в указанный параметр

                positionCount = setPositions.count;      // передает количество проставленных позиций
                indexCount = setPositions.passingCount;  // передает количество элементов, удовлетворяющих условуию                
            }
            catch
            {
                throw;
            }

            operation.Apply();                        
        }
    }
}
