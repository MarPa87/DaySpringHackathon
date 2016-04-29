using System;

namespace DaySpringApp.DataLayer
{
  public class Donation
  {
    public IdType IdType { get; set; }
    public string IdNumber { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string AddressLine3 { get; set; }
    public string PostalCode { get; set; }
    public int DonationAmount { get; set; }
    public DateTime DonationDate { get; set; }
    public string ReceiptNumber { get; set; }
    // Assume it's always cash
    public string DonationType => "O";
  }
}