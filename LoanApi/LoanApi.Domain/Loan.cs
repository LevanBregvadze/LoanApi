using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanApi.Domain
{
    public class Loan
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string LoanType { get; set; }
        public int Ammount { get; set; }
        public string Currency { get; set; }
        public int LoanPeriod { get; set; }
        public string Status { get; set; }
        public int CustomerId { get; set; }


    }
}
