using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Collections;

namespace ChatDemo
{
    public class Status
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public static Status OnSuccess(string message)
        {
            return new Status() { IsSuccess = true, Message = message };
        }

        public static Status OnFailure(string message)
        {
            return new Status() { IsSuccess = false, Message = message };
        }
    }

    public class ConnectedUser
    {
        public string UserName { get; set; }    
        public string SessionId { get; set; }
    }

    public class ConnectedUserList : List<ConnectedUser>
    {
        public ConnectedUser FindBySessionId(string sessionId)
        {
            var query = this.Where(u => u.SessionId == sessionId).FirstOrDefault();
            return query;
        }

        public ConnectedUser FindByUserId(string userName)
        {
            var query = this.Where(u => u.UserName == userName).FirstOrDefault();
            return query;
        }

        public void AddUser(string userName, string sessionId)
        {
            this.Add(new ConnectedUser() { SessionId = sessionId, UserName = userName });
        }
    }

    public class ChatMessageHelper
    {
        private static string getFormattedUserName(string userName)
        {
            return "<strong>" + userName.ToUpper() + "</strong>";
        }
        
        public static string OnMemberJoined(string userName)
        {
            return getFormattedUserName(userName) + " has joined the conversation. <br/>";
        }

        public static string OnMemberLeft(string userName)
        {
            return getFormattedUserName(userName) + " has left the conversation. <br/>";
        }

        public static string OnMessage(string userName, string message)
        {
            return getFormattedUserName(userName) + " : " + message + ". <br/>";
        }
    }


    public class ChatHub : Hub
    {
        private static Lazy<ConnectedUserList> _userList = new Lazy<ConnectedUserList>();
      
        public Status Subscribe(string userName)
        {
            try
            {
                //Add User to connected User List
                _userList.Value.AddUser(userName, Context.ConnectionId);

                //Broadcast this message to all other connected users. 
                Clients.All.onMessage(ChatMessageHelper.OnMemberJoined(userName));

                //send a success status back to caller.
                return Status.OnSuccess(userName + " is connected.");
            }
            catch (Exception)
            {
                //oops something is wrong.
                return Status.OnFailure(userName + " is not connected.");
            }
           
        }

        public Status SendMessage(string message)
        {
            try
            {
                string userName = _userList.Value.FindBySessionId(Context.ConnectionId).UserName;

                //Broadcast this message to all other connected users. 
                Clients.All.onMessage(ChatMessageHelper.OnMessage(userName, message));

                //send a success status back to caller.
                return Status.OnSuccess(userName + " message sent successfully");
            }
            catch (Exception)
            {
                return Status.OnFailure(" message is not sent");
            }
        }
    }
}