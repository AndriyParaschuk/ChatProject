using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChatProject.BL;
using ChatProject.BL.Models;
using ChatProject.DAL.Core;
using ChatProject.DAL.Repository;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ChatProject.Web.Models
{
    [HubName("chatHub")]
    public class ChatHub : Hub
    {
        //UserRepository _userRepository;

        //public ChatHub(UserRepository userRepository)
        //{
        //    _userRepository = userRepository;
        //}

        public void SendMessage(Message message, string userId, string toWhomId)
        {
            //Clients.All.addMessage(message);
            Clients.User(userId).addMessage(message);
            Clients.User(toWhomId).addMessage(message);
        }

        public void AcceptRequest(User user, User userFriend)
        {
            Clients.User(user.Id).updateAcceptedRequest(user, userFriend);
            Clients.User(userFriend.Id).updateAcceptedRequest(user, userFriend);
           // Clients.All.updateAcceptedRequest(user, userFriend);
        }

        public void DeclineRequest(User user, User userFriend)
        {
            Clients.User(user.Id).updateDeclineRequest(user, userFriend);
            Clients.User(userFriend.Id).updateDeclineRequest(user, userFriend);
            //Clients.All.updateDeclineRequest(user, userFriend);
        }
        
        public void CreateRequest(User user, User userToWhomSendRequest)
        {
            Clients.User(user.Id).createNewRequest(user, userToWhomSendRequest);
            Clients.User(userToWhomSendRequest.Id).createNewRequest(user, userToWhomSendRequest);
            //Clients.All.createNewRequest(user, userToWhomSendRequest);
        }

        public void Enter(User user)
        {
            Clients.All.enterUser(user);
            //Clients.All.createNewRequest(user, userToWhomSendRequest);
        }


        public void Exit(User user)
        {
            Clients.All.exitUser(user);
            //Clients.All.createNewRequest(user, userToWhomSendRequest);
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