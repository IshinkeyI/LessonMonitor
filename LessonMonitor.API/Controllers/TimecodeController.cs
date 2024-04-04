using LessonMonitor.API.Model;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace LessonMonitor.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimecodeController : ControllerBase
    {
        private readonly Timecode[] timecodes;

        public TimecodeController()
        {
            timecodes = new Timecode[]
            {
                new Timecode()
                {
                    Id = 1,
                    Name = "start",
                    Time = DateTime.Now.AddDays(1),
                },
                new Timecode()
                {
                    Id = 2,
                    Name = "center",
                    Time = DateTime.Now.AddDays(10),
                },
                new Timecode()
                {
                    Id = 3,
                    Name = "end",
                    Time = DateTime.Now.AddDays(100),
                },
            };
        }

        [HttpGet("GetTimecodeById")]
        public Timecode Get(int id)
        {
            var timecode = timecodes.FirstOrDefault(x => x.Id == id);

            if (timecode == null)
                throw new NullReferenceException($"Value by id {id} is null");

            return timecode;
        }

        [HttpGet("NamespaceMetadata")]
        public IEnumerable<NamespaceMetadata> GetMetadata()
        {
            var dates = new List<NamespaceMetadata>();
            var assembly = Assembly.GetAssembly(this.GetType());

            if(assembly == null) return dates;

            var classes = assembly.DefinedTypes.
                Where(t => t.FullName != null && 
                                            t.FullName.Contains("LessonMonitor") &&
                                            !t.Name.Contains("<>c"));
            
            foreach (var customClass in classes)
            {
                var classMetadata = new NamespaceMetadata() 
                {
                    ClassName = customClass.Name,
                    ClassProperties = 
                    new List<ClassPropertiesMetadata>(customClass.GetProperties().Length)
                };
                
                foreach(var property in customClass.GetProperties())
                {
                    var attribute = property.GetCustomAttribute<DiscriptionAttribute>();
                    
                    var descriptionText = "";
                    
                    if (attribute != null)
                        descriptionText = attribute.Value;

                    classMetadata.ClassProperties.Add(
                        new ClassPropertiesMetadata()
                        {
                            Description = descriptionText,
                            Name = property.Name,
                            Type = property.PropertyType.ToString()
                        });
                }
            
                dates.Add(classMetadata);
            }
            return dates;
        }

    }
}
