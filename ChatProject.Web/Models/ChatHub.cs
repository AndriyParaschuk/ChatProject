using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChatProject.BL;
using ChatProject.BL.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ChatProject.Web.Models
{
    [HubName("chatHub")]
    public class ChatHub : Hub
    {
        public void SendMessage(Message message, string userId, string toWhomId)
        {
            Clients.All.addMessage(message);
        }

        public void AcceptRequest(User user, User userFriend)
        {
            Clients.All.updateAcceptedRequest(user, userFriend);
        }

        public void DeclineRequest(User user, User userFriend)
        {
            Clients.All.updateDeclineRequest(user, userFriend);
        }
        
        public void CreateRequest(User user, User userToWhomSendRequest)
        {
            Clients.All.createNewRequest(user, userToWhomSendRequest);
        }

        //public void SendMessage(string name, string message)
        //{
        //    Clients.All.broadcastMessage(name, message);
        //}

        //public void Send(string name, string message)
        //{
        //    Clients.All.addNewMessageToPage(name, message);
        //}
    }
}