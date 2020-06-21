using System;
using System.Threading.Tasks;
using BusinessLayer.Services.Debts;
using BusinessLayer.Services.Users;
using BusinessLayer.Utilities;
using DataLayer.Entities;
using MailKit.Net.Smtp;
using MimeKit;
using System.Net;
using System.Net.Mail;
using System.Collections.Generic;
using DataLayer.DataContext;
using BusinessLayer.Views;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
namespace BusinessLayer.Common.SendEmails
{
    [DisallowConcurrentExecution]
    public class SendEmailJob : IJob
    {
        //private readonly ILogger<SendEmailJob> _logger;
        private readonly IServiceProvider _serviceProvider;
        public SendEmailJob(IServiceProvider serviceProvider)
        {
            //_logger = logger;
            _serviceProvider = serviceProvider;
        }

        async Task IJob.Execute(IJobExecutionContext context)
        {
            try
            {
                using (IServiceScope scope = _serviceProvider.CreateScope())
                {
                    IIdentityService _identityService = scope.ServiceProvider.GetRequiredService<IIdentityService>();
                    IDebtService _debtService = scope.ServiceProvider.GetRequiredService<IDebtService>();
                    var users = await _identityService.GetUsers();


                    foreach (var user in users)
                    {
                        var fromAddress = new MailAddress("speedlight.money@gmail.com", "Speedlight Money");
                        var toAddress = new MailAddress(user.Email, user.FirstName + " " + user.LastName);
                        const string fromPassword = "carryON09171996";
                        const string subject = "[Speedlight Money] Due date alert";
                       

                        var debtsForUser = await _debtService.GetDebtsForUser(user.Id);
                        foreach (var debt in debtsForUser)
                        {
                            string body = "<div>Hi, " + user.FirstName + "! </div> <br/>" +
                           "<div>We are sending this email to remind you of your debt to " + debt.LenderName + ". You have 2 days until the the due date of the debt.</div><br/>" +
                           "<div>Here are some details about your debt: </div>" +
                           "<div>Owed amount: " + debt.LenderName + "</div>" +
                            "<div>Owed amount: " + debt.LoanAmount +" " + debt.CurrencyName + "</div>" +
                           "<div>Due date: " + debt.DueDate + "</div><br/>"  +
                           "<div>You can go to the Debts tab on the application and send the money to " + debt.LenderName + " by clicking on the Return loan column corresponding to "+ debt.LenderName +". If you can't return the loan right now, don't forget you can defer the debt with 14 days from the Debts tab in the Speedlight Money App.</div>" +
                           "<div>Please make sure you don't forget to pursue the changes and don't hesitate to contact us if you have any questions or concerns!</div><br/>" +
                           "<div>Thank you for using our app and have a great week!</div><br/>" +
                           "<div>Speedlight Money Team</div>" +
                           "<div>Phone: 0771345665</div>" +
                           "<div>More info: speedlightmoney.com</div>";

                            if (debt.ReturnDate == null && debt.DueDate != null && DateTime.Now.Date == debt.DueDate.Value.Date.AddDays(-2))
                            {
                                using var smtp = new System.Net.Mail.SmtpClient
                                {
                                    Host = "smtp.gmail.com",
                                    Port = 587,
                                    EnableSsl = true,
                                    DeliveryMethod = SmtpDeliveryMethod.Network,
                                    UseDefaultCredentials = false,
                                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                                };
                                using var message = new MailMessage(fromAddress, toAddress)
                                {
                                    Subject = subject,
                                    Body = body,
                                    IsBodyHtml = true
                                };
                                smtp.Send(message);
                            }
                        }
                    }


                    //if(debt)
                    //prepare email
                    //MimeMessage message = new MimeMessage();

                    //MailboxAddress from = new MailboxAddress("Speedlight Money",
                    //"speedliht.money@gmail.com");
                    //message.From.Add(from);

                    //MailboxAddress to = new MailboxAddress("User",
                    //"user@example.com");
                    //message.To.Add(to);

                    //message.Subject = "This is email subject";

                    //add email body
                    //BodyBuilder bodyBuilder = new BodyBuilder();
                    //bodyBuilder.HtmlBody = "<h1>Hello World!</h1>";
                    //bodyBuilder.TextBody = "Hello World!";

                    //message.Body = bodyBuilder.ToMessageBody();

                    //SmtpClient client = new SmtpClient();
                    //client.Connect("smtp_address_here", 8080, true);
                    //client.Authenticate("user_name_here", "pwd_here");
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}