using MyTaskManager.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyTaskManager.DTO
{
    public class MyTaskDto
    {
        [JsonIgnore]
        public int Id { get; set; }

        [MaxLength(50)]
        public string TitleTask { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string CategoryDescription { get; set; } = string.Empty;
        public myPriority Priority { get; set; }

        [JsonIgnore]
        public string PriorityDescription { get; set; }
        public DateTime Expiration { get; set; }
        //public string UserLastName { get; set; } = string.Empty;
        //public string UserFirstName { get; set; } = string.Empty;

        public enum myPriority : byte
        {
            Низкий,
            Нормальный,
            Высокий
        }

        public MyTaskDto()
        {
            PriorityDescription = GetDescription((int)Priority);
        }

        private string GetDescription(int id)
        {
            string[] descriptionVariants =
                [
                    "обычно относится к задачам или действиям, которые имеют низкую степень важности или срочности. " +
                    "Это могут быть задачи, которые могут быть выполнены во второстепенном порядке или отложены в пользу более важных задач.",

                    "обычно относится к задачам или действиям, которые имеют среднюю степень важности или срочности. " +
                    "Это могут быть задачи, которые требуют выполнения в разумные сроки, но не являются критически важными.",

                    "обычно относится к задачам или действиям, которые имеют высокую степень важности или срочности. " +
                    "Это могут быть критически важные задачи, которые требуют немедленного внимания и выполнения."
                ];

            return descriptionVariants[id];
        }
    }
}