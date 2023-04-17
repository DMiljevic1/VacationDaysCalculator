using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Models
{
	public class MedicalCertificate
	{
		public int Id { get; set; }
		public DateTime MedicalCertificateDate { get; set; }
		public byte[]? Attachment { get; set; }
		public int SickLeaveId { get; set; }
		public SickLeave? SickLeave { get; set; }
	}
}
