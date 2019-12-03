﻿using System;
using System.Collections.Generic;
using GmailQuickstart;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using SaintSender.Core.Entities;

namespace SaintSender.Core.Services
{
    public partial class EmailService
    {
        private readonly GmailService _gmail;

        public EmailService()
        {
            _gmail = Gmail.GetService();
        }

        public List<Email> GetEmails()
        {
            List<Email> emails = new List<Email>();

            UsersResource.MessagesResource.ListRequest messageRequest = _gmail.Users.Messages.List("me");

            messageRequest.MaxResults = 20;
            messageRequest.IncludeSpamTrash = true;
            messageRequest.LabelIds = "INBOX";

            ListMessagesResponse messageResponse = messageRequest.Execute();

            foreach (Message message in messageResponse.Messages)
            {
                Message messageInfo = _gmail.Users.Messages.Get("me", message.Id).Execute();

                Email email = BuildEmail(messageInfo, message.Id);
                emails.Add(email);
            }

            return emails;
        }

    }
}
