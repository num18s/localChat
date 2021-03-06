﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace localChat
{
    public class User
    {
        private string imsi, username, email;
        private long imsiID, userID;
        private int status;
        private DateTime createDate, modifyDate;

        public User(long imsiID, long userID, string imsi, string username, string email, int status, DateTime createDate, DateTime modifyDate)
        {
            this.imsiID = imsiID;
            this.userID = userID;
            this.imsi = imsi;
            this.username = username;
            this.status = status;
            this.email = email;
            this.createDate = createDate;
            this.modifyDate = modifyDate;
        }

        public User copy()
        {
            return new User(imsiID, userID, imsi, username, email, status, createDate, modifyDate);
        }

        public long getImsiID() { return imsiID; }

        public long getUserID() { return userID; }

        public string getImsi(){ return imsi; }

        public string getUsername() { return username; }

        public string getEmail() { return email; }

        public int getStatus() { return status; }

        public DateTime getCreateDate() { return createDate; }

        public DateTime getModifyDate() { return modifyDate; }
    }
}
