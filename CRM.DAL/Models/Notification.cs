using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DAL.Models
{
    public enum NotificationTypeEnum
    {
        LeadAssigned = 1,
        CustomerAssigned = 2,
        LeadConverted = 3,
        TaskAssigned = 4,
        TaskDueSoon = 5,
        TaskOverdue = 6,
        OpportunityWon = 7,
        OpportunityLost = 8,
        PaymentReceived = 9,
        PaymentFailed = 10,
        InvoiceOverdue = 11,
        General = 99
    }
    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public NotificationTypeEnum NotificationType { get; set; } = NotificationTypeEnum.General;
        public bool IsRead { get; set; } = false;
        public DateTime? ReadAt { get; set; }
        public string RecipientUserId { get; set; }
        public ApplicationUser Recipient {  get; set; }
    }
}
