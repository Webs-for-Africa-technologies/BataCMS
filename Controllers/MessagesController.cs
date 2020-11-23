using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using COHApp.Data.Interfaces;
using COHApp.Data.Models;
using COHApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace COHApp.Controllers
{
    public class MessagesController : Controller
    {
        private readonly IMessagesRepository _messageRepository;


        public MessagesController(IMessagesRepository messagesRepository)
        {
            _messageRepository = messagesRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult List()
        {
            IEnumerable<Message> messages = _messageRepository.Messages;

            return View(messages);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AddAsync()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(AddMessageViewModel model)
        {
            if (ModelState.IsValid)
            {
                Message message = new Message
                {
                    MessageTitle = model.MessageTitle,
                    MessageContent = model.MessageContent,
                    DateModified = DateTime.Now
                };

                await _messageRepository.AddMessageAsync(message);

                return RedirectToAction("Complete", new { message = "The Message is added Successfully and is now available for viewing" });
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EditAsync(int id)
        {
            var message = await _messageRepository.GetMessageByIdAsync(id);

            if (message == null)
            {
                ViewBag.ErrorMessage = $"User with message with id ={id} cannot be found";
                return View("NotFound");
            }

            var model = new EditMessageViewModel
            {
                Id = message.MessageId,
                MessageTitle = message.MessageTitle,
                MessageContent = message.MessageContent
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(EditMessageViewModel model)
        {
            var message = await _messageRepository.GetMessageByIdAsync(model.Id);

            if (message == null)
            {
                ViewBag.ErrorMessage = $"Message with messageId ={model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                message.MessageTitle = model.MessageTitle;
                message.MessageContent = model.MessageContent;
                message.DateModified = DateTime.Now;


                await _messageRepository.UpdateMessageAsync(message);
                return RedirectToAction("Complete", new { message = "The Message was updated successfully and is now available for viewing" });

            }
        }


        public async Task<IActionResult> Delete(int id)
        {
            var message = await _messageRepository.GetMessageByIdAsync(id);

            if (message == null)
            {
                ViewBag.ErrorMessage = $"The Message with MessageId={id} cannot be found";
                return View("NotFound");
            }
            else
            {
                await _messageRepository.DeleteMessageAsync(message);

                return RedirectToAction("Complete", new { message = "The Message was deleted successfully" });
            }
        }


        public IActionResult Complete(string message)
        {
            ViewBag.Message = message;
            return View();
        }
    }
}
