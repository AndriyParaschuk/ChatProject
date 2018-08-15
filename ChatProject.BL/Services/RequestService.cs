using ChatProject.BL.Interfaces;
using ChatProject.BL.Interfaces.Services;
using ChatProject.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatProject.BL.Services
{
    public class RequestService : IRequestService
    {
        IUserFriendRepository _userFriendRepository;
        IUserRepository _userRepository;
        IRequestRepository _requestRepository;
        IMessageRepository _messageRepository;

        public RequestService(IUserFriendRepository userFriendRepository, IUserRepository userRepository, IRequestRepository requestRepository, IMessageRepository messageRepository)
        {
            _userFriendRepository = userFriendRepository;
            _userRepository = userRepository;
            _requestRepository = requestRepository;
            _messageRepository = messageRepository;
        }

        public void AcceptRequest(string userId, string withWhomId)
        {
            Request currentRequest = _requestRepository.GetAll().FirstOrDefault(item => item.FromId == withWhomId && item.ToId == userId);
            currentRequest.Status = RequestStatus.Accept;
            _requestRepository.Update(currentRequest);
            _requestRepository.SaveChanges();

            UserFriend currentUserFriend = new UserFriend() { FriendId = withWhomId, UserId = userId };
            _userFriendRepository.Create(currentUserFriend);
            _userFriendRepository.SaveChanges();
        }

        public void DeclineRequest(string userId, string withWhomId)
        {
            Request request = _requestRepository.GetAll().FirstOrDefault(item => item.FromId == withWhomId && item.ToId == userId);
            request.Status = RequestStatus.Decline;
            _requestRepository.Update(request);
            _requestRepository.SaveChanges();
        }

        public void CreateRequest(string userId, string toWhomId)
        {
            _messageRepository.Create(new Message() { FromId = userId, ToId = toWhomId, TextMessage = "Hi " + 
                _userRepository.GetById(toWhomId).UserName + ") I want to add you to my friend" });
            _messageRepository.SaveChanges();

            _requestRepository.Create(new Request() { FromId = userId, ToId = toWhomId });
            _requestRepository.SaveChanges();
        }
    }
}
