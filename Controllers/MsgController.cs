using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MG_5_FreelanceJobsite.Models;

namespace MG_5_FreelanceJobsite.Controllers
{
    public class MsgController : Controller
    {
        /*
         * Message creation for persisted data
         */
        [Route("MsgController/PersistedMsgCreation")]
        [HttpPost]
        public ActionResult PersistedMsgCreation([Bind(Include = "Id, SenderId, ReceiverId, Subject, JobId, Message, Date")]Messaging model)
        {
            using (dbEntities db = new dbEntities())
            {
                Messaging OldData = (Messaging)TempData["Message"]; //persisted data from crontroller action that calls the view that calls this
                if (ModelState.IsValid)
                {
                    int Msgid = db.Messagings.Count();
                    int senderid = (int)Session["loginid"];
                    Messaging temp = new Messaging()
                    {
                        Id = Msgid + 1,
                        Date = model.Date,
                        SenderId = senderid,
                        ReceiverId = model.ReceiverId,
                        Subject = model.Subject,
                        JobId = model.JobId,
                        Message = model.Message
                    };

                    //db.Messagings.Add(temp);  in case approach below is incorrect
                    db.Users.Where(o => o.Id == senderid).Single().Messagings1.Add(temp);
                    db.SaveChanges();

                    //make another message for the receiver of the message. must do this since the two messages must have different message ids
                    Msgid = db.Messagings.Count();
                    Messaging temp2 = new Messaging()
                    {
                        Id = Msgid + 1,
                        Date = model.Date,
                        SenderId = senderid,
                        ReceiverId = model.ReceiverId, 
                        Subject = model.Subject,
                        JobId = model.JobId,
                        Message = model.Message
                    };

                    db.Users.Where(o => o.Id == model.ReceiverId).Single().Messagings.Add(temp);
                    db.SaveChanges();
                    return RedirectToAction("MessagePageDisplayStd", "Msg");
                }

                return View(); //redirect to partial view possibly for error handling 
            }
        }

        /*
         * Standard message creation  
         */
        [Route("MsgController/CreateMsg/{model}")]
        [HttpPost]
        public ActionResult CreateMsg(UserMessagesVeiwModel model)  
        {
            using (dbEntities db = new dbEntities())
            {
                Boolean ValidEmail = false;
                List<User> users = db.Users.ToList();
                int receiverid=-1;
                if(model.Employerid!=null)
                {
                    receiverid = (int)model.Employerid;
                }
                foreach(var u in users) 
                {
                    if(u.Email.Equals((string)model.Employeremail))
                    {
                        ValidEmail = true;
                        receiverid = u.Id;
                    }
                }
                if (ModelState.IsValid && ValidEmail==true) 
                {
                    int senderid = (int)Session["loginid"];
                    int Msgid = db.Messagings.Count();
                    Messaging temp = new Messaging()
                    {
                        Id = Msgid + 1,
                        Date = DateTime.Now, 
                        SenderId = senderid, 
                        ReceiverId = receiverid,
                        Subject = model.Msg.Subject,
                        JobId = model.Msg.JobId,
                        Message = model.Msg.Message
                    };
                    db.Users.Where(o => o.Id == senderid).Single().Messagings1.Add(temp);
                    db.SaveChanges();

                    //make another message for the receiver of the message. must do this since the two messages must have different message ids
                    Msgid = db.Messagings.Count();
                    Messaging temp2 = new Messaging()
                    {
                        Id = Msgid + 1,
                        Date = DateTime.Now, 
                        SenderId = senderid,
                        ReceiverId = receiverid,
                        Subject = model.Msg.Subject,
                        JobId = model.Msg.JobId,
                        Message = model.Msg.Message
                    };

                    db.Users.Where(o => o.Id == receiverid).Single().Messagings.Add(temp);
                    db.SaveChanges();
                    return RedirectToAction("MessagePageDisplayStd", "Msg"); 
                }
                model.ErrorLoadingPage = "Please enter a valid email";
                return View("~/Views/Msg/MsgView.cshtml", model); //redirect to message page with error output on page as well
            }
        }

        /*
         * Method called when redirecting to view of message page  
         */
        [Route("MsgController/MessagePageDisplayStd")]
        public ActionResult MessagePageDisplayStd()
        {
            using (dbEntities db = new dbEntities())
            {
                int user = (int)Session["loginid"];
                UserMessagesVeiwModel temp = new UserMessagesVeiwModel();
                temp.Inbox = new List<Messaging>();
                temp.Sent = new List<Messaging>();
                List<Messaging> x = db.Users.Where(o => o.Id == user).Single().Messagings.ToList();
                List<Messaging> y = db.Users.Where(o => o.Id == user).Single().Messagings1.ToList();
                foreach(var t in x)
                {
                    temp.Inbox.Add(t);
                }
                foreach(var q in y)
                {
                    temp.Sent.Add(q);
                }
                temp.Jobtitle = null;
                temp.Employerid = null;
                temp.Msg = new Messaging();
                temp.ErrorLoadingPage = null;
                return View("~/Views/Msg/MsgView.cshtml", temp);
            }
        }
        
        /*
         * Method called to display a specifc message selected from the user in their inbox
         */
        [Route("MsgController/DisplayMessage/{id}")]
        [HttpGet] 
        public ActionResult DisplayMessage(int id)  //passed in by the current id for the message to be viewed
        {
            using (dbEntities db = new dbEntities())
            {
                SingleMessageDisplayViewModel message = new SingleMessageDisplayViewModel
                {
                    Msg=new Messaging()
                };
                message.Msg = db.Messagings.Where(o => o.Id == id).Single();
                int sender = message.Msg.SenderId;
                int user = (int)Session["loginid"];
                if(sender != (int)Session["loginid"])
                {
                    int y = message.Msg.ReceiverId;
                    message.ReceipientName = db.Users.Where(o => o.Id == y).Single().FirstName + " " +
                        db.Users.Where(o => o.Id == y).Single().LastName; ;
                    string t = db.Users.Where(o => o.Id == user).Single().FirstName + " " +
                        db.Users.Where(o => o.Id == user).Single().LastName;
                    message.SenderName = t;
                }
                else
                {
                    int te = message.Msg.SenderId;
                    string w = db.Users.Where(o => o.Id == te).Single().FirstName + " " +
                        db.Users.Where(o => o.Id == te).Single().LastName;
                    message.SenderName = w;
                    message.ReceipientName = db.Users.Where(o => o.Id == sender).Single().FirstName + " " +
                        db.Users.Where(o => o.Id == sender).Single().LastName;
                }
                return View("~/Views/Msg/MsgDisplayView.cshtml", message);
            }
        }

        /*
         * Controller method for std (non-auto formatted) message compostion
         */
        [Route("MsgController/ComposeMsg")]
        public ActionResult ComposeMsg()
        {
            using (dbEntities db = new dbEntities())
            {
                int user = (int)Session["loginid"];
                UserMessagesVeiwModel temp = new UserMessagesVeiwModel();
                temp.Inbox = new List<Messaging>();
                temp.Sent = new List<Messaging>();
                List<Messaging> x = db.Users.Where(o => o.Id == user).Single().Messagings.ToList();
                List<Messaging> y = db.Users.Where(o => o.Id == user).Single().Messagings1.ToList();
                foreach (var t in x)
                {
                    temp.Inbox.Add(t);
                }
                foreach (var q in y)
                {
                    temp.Sent.Add(q);
                }
                temp.Jobtitle = null;
                temp.Employerid = null;
                temp.Msg = new Messaging();
                temp.ErrorLoadingPage = null;
                return View("~/Views/Msg/ComposeMsgView.cshtml", temp);
            }
        }

        /*
         * Autofill version of MessagePageDisplay which is called if a message is specifically selected from a job page by a user that is not the 
         * employer of the job
         */
        [Route("MsgController/MessagePageDisplayAuto/{id}")]
        [HttpGet]
        public ActionResult MessagePageDisplayAuto(int Jobid)
        {
            using (dbEntities db = new dbEntities())
            {
                int user = (int)Session["loginid"];
                UserMessagesVeiwModel temp = new UserMessagesVeiwModel();
                temp.Inbox = new List<Messaging>();
                temp.Sent = new List<Messaging>();
                List<Messaging> x = db.Users.Where(o => o.Id == user).Single().Messagings.ToList();
                List<Messaging> y = db.Users.Where(o => o.Id == user).Single().Messagings1.ToList();
                foreach (var t in x)
                {
                    temp.Inbox.Add(t);
                }
                foreach (var q in y)
                {
                    temp.Sent.Add(q);
                }
                temp.Jobtitle = db.Jobs.Where(o => o.Id == Jobid).Single().Title;
                temp.Msg.ReceiverId = db.Jobs.Where(o => o.Id == Jobid).Single().EmployerId;
                temp.Msg = null;
                temp.ErrorLoadingPage = null;
                return View("~/Views/Msg/ComposeMsgView.cshtml", temp);
            }
        }
    }
}
