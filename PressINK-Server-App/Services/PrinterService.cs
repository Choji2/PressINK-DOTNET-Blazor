using PressINK_Server_App.Data.API_Forms;
using PressINK_Server_App.Model.API_Form_Models;
using PressINK_Server_App.Model.PrinterPingModel;
using DB_SCHEMA.Data.PrinterContexts;
using Microsoft.EntityFrameworkCore;
using Web_scraper_practice_.Model.Printer_DB_Models;
using Web_scraper_practice_.Model.Printer_DB_Models.Sub_models;

namespace PressINK_Server_App.Services
{
    public class PrinterService
    {
        private IDbContextFactory<PrinterContext> _dbContextFactory;
        private IDbContextFactory<API_FormContext> _dbContextFactory2;
        private ILogger<PrinterService> _logger;
        public PrinterService( IDbContextFactory<PrinterContext> dbContextFactory, IDbContextFactory<API_FormContext> dbContextFactory2, ILogger<PrinterService> logger)
        {
            _dbContextFactory = dbContextFactory;
            _dbContextFactory2 = dbContextFactory2;
            _logger = logger;
        }


        //GET
        public List<PRINTER_MAIN> GetPrinters()
        {
            var context = _dbContextFactory.CreateDbContext();        
            return context.PRINTER_MAIN.ToList();
            
        }

        public PRINTER_MAIN GetPrinterByID(int id)
        {
            var context = _dbContextFactory.CreateDbContext(); 
            return context.PRINTER_MAIN.SingleOrDefault(x => x.ID == id);
                
            
        }
    
        public PRINTER_MAIN GetPrinterByQue(string que)
        {
            var context = _dbContextFactory.CreateDbContext();
            var printer = context.PRINTER_MAIN.SingleOrDefault(x => x.QUE == que);
            return printer;      
        }

        public List<PRINTER_MAIN> GetPrinterByHostname(string hostname)
        {
            var context = _dbContextFactory.CreateDbContext();
            var list = GetPrinters();
            var printers = list.Where(x=>x.HOSTNAMEv4.Contains(hostname));
            return (List<PRINTER_MAIN>)printers;
            
        }

        public List<PRINTER_MAIN> GetPrintersByModel(string model)
        {

            var context = _dbContextFactory.CreateDbContext();
            var list = GetPrinters();
            var printers = list.Where(x => x.HOSTNAMEv4.Contains(model));
            return (List<PRINTER_MAIN>)printers;
            
        }

        public List<PRINTER_MAIN> GetPrinterByAsset(string asset)
        {
            var context = _dbContextFactory.CreateDbContext();            
            var list = GetPrinters();
            var printers = list.Where(x => x.ASSET_NUMBER.Contains(asset));
            return (List<PRINTER_MAIN>)printers;         
        }

        public List<PRINTER_MAIN> GetPrintersByPlant(string plantID)
        {
            var context = _dbContextFactory.CreateDbContext();   
            var list = GetPrinters();
            var printers = list.Where(x => x.PLANT_ID.Equals(plantID));
            Console.WriteLine(printers.ToList());
            return printers.ToList(); 
        }

        public async Task<List<Attribute_Type>> GetAllAttributeTypes()
        {
            var context = _dbContextFactory2.CreateDbContext();
            return await context.Attribute_Type.ToListAsync();
        }


        public async Task<List<MODEL>> GetModels()
        {
            var context = _dbContextFactory.CreateDbContext();
            return await context.MODEL.ToListAsync();
            
        }
        public MODEL GetModelByID(string id)
        {
            var context = _dbContextFactory.CreateDbContext();     
            return context.MODEL.SingleOrDefault(x => x.MODEL_ID == id);        
        }
  
        public List<PLANT> GetPlants()
        {
            var context = _dbContextFactory.CreateDbContext();           
            return context.PLANT.ToList();
            
        }
        public PLANT GetPlantByID(string id)
        {
            var context = _dbContextFactory.CreateDbContext();          
            return context.PLANT.SingleOrDefault(x => x.PLANT_ID == id);          
        }

        public List<MANUFACTOR> GetManufactors()
        {
            var context = _dbContextFactory.CreateDbContext();
            return context.MANUFACTURE.ToList();        
        }

        public MANUFACTOR GetManufactorByID(string id)
        {
            var context = _dbContextFactory.CreateDbContext();      
            return context.MANUFACTURE.SingleOrDefault(x => x.MANUFACTOR_ID == id);
        }

        public List<CATEGORY> GetCategories()
        {
            var context = _dbContextFactory.CreateDbContext();          
            return context.CATEGORIES.ToList();         
        }

        public async Task<List<API_Templates>> GetTemplates()
        {
            var context = await _dbContextFactory2.CreateDbContextAsync();
            return await context.API_Templates.ToListAsync();
        }

        public async Task<API_Templates> GetTemplate(string temp)
        {
            var context = await _dbContextFactory2.CreateDbContextAsync();
            return await context.API_Templates.FirstOrDefaultAsync(x=> x.ID==temp);

        }


        public async Task<List<API_Template_Attributes>> GetTemplateAttributes(string template_ID)
        {
            var context = await _dbContextFactory2.CreateDbContextAsync();
            return await context.API_Template_Attributes.Where(x=>x.API_Templates.ID == template_ID).ToListAsync();

        }

        public async Task<List<Model_Attributes>> GetAllAttributes()
        {
            var context = await _dbContextFactory2.CreateDbContextAsync();
            return await context.Model_Attributes.ToListAsync();
        }

        public async Task<List<Model_Attributes>> GetAllModelAttributes(string modelID)
        {
            var context = await _dbContextFactory2.CreateDbContextAsync();
            return await context.Model_Attributes.Where(x=>x.Model_ID == modelID).ToListAsync();

        }

        public async Task<Model_Attributes> GetModelAttribute(int attributeID)
        {
            var context = await _dbContextFactory2.CreateDbContextAsync();
            return await context.Model_Attributes.SingleOrDefaultAsync(x => x.ID == attributeID);

        }


        //Create
        public void AddPrinter(PRINTER_MAIN printer)
        {
            var context = _dbContextFactory.CreateDbContext();          
            context.PRINTER_MAIN.Add(printer);
            context.SaveChanges();        
        }

        public async void AddModel(MODEL model)
        {
            var context = _dbContextFactory.CreateDbContext();           
            context.MODEL.Add(model);
            context.SaveChanges();   
            

            var context2 = await  _dbContextFactory2.CreateDbContextAsync();
            var attributeList = await GetTemplateAttributes(model.API_template);

            if (attributeList != null) {
                try
                {
                    foreach (var attribute in attributeList)
                    {
                        await context2.Model_Attributes.AddAsync(
                            new Model_Attributes
                            {
                                Attribute_Name = attribute.Attribute_Name,
                                Attribute_Type = attribute.Attribute_Type,
                                Attribute_Value = "",
                                Model_ID = model.MODEL_ID
                            });    
                    }
                    var data = context2.SaveChanges();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,ex.Message);
                }             
            }
        }

        public void AddPlant(PLANT plant)
        {
            var context = _dbContextFactory.CreateDbContext();        
            context.PLANT.Add(plant);
            context.SaveChanges();         
        }

        public void AddManufactor(MANUFACTOR manufactor)
        {
            var context = _dbContextFactory.CreateDbContext();           
            context.MANUFACTURE.Add(manufactor);
            context.SaveChanges();        
        }

  

        //Update
        public void UpdatePrinter(PRINTER_MAIN updatedprinter)
        {         
            var printer = GetPrinterByID(updatedprinter.ID);
            if(printer == null)
            {
                throw new Exception("Printer does not exist. Cannot update.");
            }
            else
            {
                printer.HOSTNAMEv4= updatedprinter.HOSTNAMEv4;
                printer.ASSET_NUMBER= updatedprinter.ASSET_NUMBER;
                printer.QUE= updatedprinter.QUE;
                printer.MODEL_ID= updatedprinter.MODEL_ID;
                printer.LOCATION= updatedprinter.LOCATION;
                printer.PLANT_ID= updatedprinter.PLANT_ID;
                printer.CATEGORY_ID= updatedprinter.CATEGORY_ID;

                var context = _dbContextFactory.CreateDbContext();               
                context.Update(printer);
                context.SaveChanges();             
            }                
        }

        public async Task UpdateModel(MODEL updatedModel,List<Model_Attributes> attributeList)
        {
            var model = GetModelByID(updatedModel.MODEL_ID);
            if (model == null)
            {
                throw new Exception("Model does not exist. Cannot update.");
            }
            else
            {
                model.MODEL_NAME= updatedModel.MODEL_NAME;
                model.MANUFACTOR_ID= updatedModel.MANUFACTOR_ID;
                model.DESCP= updatedModel.DESCP;
                model.LINK= updatedModel.LINK;

                var context = _dbContextFactory.CreateDbContext();             
                context.Update(model);
                context.SaveChanges();

                var context2 = _dbContextFactory2.CreateDbContext();
                foreach (var attribute in attributeList) { 
                
                    var old_attribute = await GetModelAttribute(attribute.ID);

                    old_attribute.Attribute_Value = attribute.Attribute_Value;
                    old_attribute.Attribute_Type = attribute.Attribute_Type;

                    context2.Update(old_attribute);        
                }
                context2.SaveChanges();             
            }


        }

        public void UpdateManufactor(MANUFACTOR updatedManufactor)
        {
            var manufactor = GetManufactorByID(updatedManufactor.MANUFACTOR_ID);
            if (manufactor == null)
            {
                throw new Exception("Printer does not exist. Cannot update.");
            }
            else
            {
                manufactor.NAME = updatedManufactor.NAME;
                manufactor.DESCP= updatedManufactor.DESCP;

                var context = _dbContextFactory.CreateDbContext();             
                context.Update(manufactor);
                context.SaveChanges();
                
            }


        }

        public void UpdatePlant(PLANT updatedPlant)
        {
            var plant = GetPlantByID(updatedPlant.PLANT_ID);
            if (plant == null)
            {
                throw new Exception("Printer does not exist. Cannot update.");
            }
            else
            {
                plant.NAME= updatedPlant.NAME;
                plant.DESCP= updatedPlant.DESCP;

                var context = _dbContextFactory.CreateDbContext();              
                context.Update(plant);
                context.SaveChanges();
                
            }
        }

        public async Task UpdateModelAttribute(Model_Attributes new_attribute)
        {
            var context = _dbContextFactory2.CreateDbContext();
            Model_Attributes old_attribute= await GetModelAttribute(new_attribute.ID);
            if (old_attribute != null)
            {
                old_attribute.Attribute_Value = new_attribute.Attribute_Value;
                old_attribute.Attribute_Name = new_attribute.Attribute_Name;
                old_attribute.Attribute_Type = new_attribute.Attribute_Type;
                context.Model_Attributes.Update(old_attribute);
            }
            
            context.SaveChanges();
        }

        //Delete
        public void DeletePrinter(int id )
        {
            var printer = GetPrinterByID(id);
            if (printer == null)
            {
                throw new Exception();
            }
            var context = _dbContextFactory.CreateDbContext();
            context.Remove(printer);
            context.SaveChanges();      
        }

        public async void DeleteModel(string id)
        {
            var model = GetModelByID(id);
            if (model != null)
            {
                var context = _dbContextFactory.CreateDbContext();
                context.Remove(model);
                context.SaveChanges();

                var context2 = _dbContextFactory2.CreateDbContext();
                var attributeList = await GetAllModelAttributes(id);
                foreach (var attribute in attributeList)
                {
                    context2.Model_Attributes.Remove(attribute);
                }
                context2.SaveChanges();
            }
        }

        public void DeleteManufactor(string id)
        {
            var manufactor = GetManufactorByID(id);
            if (manufactor == null)
            {
                throw new Exception();
            }

            var context = _dbContextFactory.CreateDbContext();           
            context.Remove(manufactor);
            context.SaveChanges();
            
        }

        public void DeletePlant(string id)
        {
            var plant = GetPlantByID(id);
            if (plant == null)
            {
                throw new Exception();
            }

            var context = _dbContextFactory.CreateDbContext();           
            context.Remove(plant);
            context.SaveChanges();            
        }




    }
}
