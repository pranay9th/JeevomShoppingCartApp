﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JeevomShoppingCard.ViewModels
{
    public class UserSession
    {
        public String SessionId { get; set; }
        public DateTime CreatedDateTime { get; set; }

        public UserSession(String sessionId)
        {
            this.SessionId = sessionId;
            this.CreatedDateTime = DateTime.Now;
        }
    }
}