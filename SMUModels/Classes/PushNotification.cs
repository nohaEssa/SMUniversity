using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMUModels.Classes
{
    public class PushNotification
    {

        public static bool Push(long StudentID, long LecturerID, string TitleAr, string TitleEn, string DescriptionAr, string DescriptionEn, int NotificationTypeID)
        {
            admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

            bool Result = false;
            List<TblRegisterDevice> Tokens = new List<TblRegisterDevice>();
            if (StudentID > 0)
            {
                Tokens = _Context.TblRegisterDevices.Where(a => a.StudentID == StudentID).ToList();
            }
            else if (LecturerID > 0)
            {
                Tokens = _Context.TblRegisterDevices.Where(a => a.LecturerID == LecturerID).ToList();
            }

            foreach (var item in Tokens)
            {
                //string category = string.Format("{0}#${1}", Code, NotificationTypeID);
                string category = string.Format("{0}#${1}", 0, NotificationTypeID);
                if (item != null && !string.IsNullOrEmpty(item.Token))
                {
                    if (item.BadgeNo > 0)
                        item.BadgeNo++;
                    else
                        item.BadgeNo = 1;

                    if (item.DeviceTypeID == 1)
                    {
                        //Diyafa.Business.aps snd = new Diyafa.Business.aps();
                        //snd.alertAR = strMessageAr;
                        //snd.alertEN = strMessageEn;
                        //snd.category = category;
                        //snd.badge = 1;//d.BadgeNo.HasValue ? d.BadgeNo.Value : 0;
                        //snd.sound = "Default";
                        //PushAndroid(JsonConvert.SerializeObject(snd), item.Token, snd.badge);
                        Result = AndroidGCMPushNotification.SendNotification(item.Token, TitleAr, TitleEn, DescriptionAr, DescriptionEn);
                    }
                    else
                    {
                        //var push = new PushBroker();

                        //push.OnNotificationSent += NotificationSent;
                        //push.OnChannelException += ChannelException;
                        //push.OnServiceException += ServiceException;
                        //push.OnNotificationFailed += NotificationFailed;
                        //push.OnDeviceSubscriptionExpired += DeviceSubscriptionExpired;
                        //push.OnDeviceSubscriptionChanged += DeviceSubscriptionChanged;
                        //push.OnChannelCreated += ChannelCreated;
                        //push.OnChannelDestroyed += ChannelDestroyed;


                        //string Certificates = ConfigurationManager.AppSettings["Certificates"];

                        //var appleCert = File.ReadAllBytes(System.Web.HttpContext.Current.Server.MapPath("~/" + Certificates));

                        //push.RegisterAppleService(new ApplePushChannelSettings(false, appleCert, System.Configuration.ConfigurationManager.AppSettings["Password"], true));

                        //push.QueueNotification(new AppleNotification()
                        //     .ForDeviceToken(item.Token)//the recipient device id
                        //     .WithAlert(TitleAr)//the message
                        //     .WithCategory(category)
                        //     .WithBadge(1/*d.BadgeNo.Value*/)
                        //     .WithSound("Default")
                        //     );
                        //push.StopAllServices(true);
                    }

                    _Context.SaveChanges();
                }
            }

            return Result;
        }

        //public static bool PublicPush(int TokenID, string TitleAr, string TitleEn, string DescriptionAr, string DescriptionEn, int NotificationTypeID)
        //{
        //    admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

        //    bool Result = false;
        //    TblRegisterDevice d = _Context.TblRegisterDevices.Where(a => a.Code == TokenID).SingleOrDefault();
        //    string category = string.Format("{0}#${1}", Code, NotificationTypeID);
        //    if (d != null && !string.IsNullOrEmpty(d.Token))
        //    {
        //        if (d.BadgeNo.HasValue && d.BadgeNo.Value > 0)
        //            d.BadgeNo++;
        //        else
        //            d.BadgeNo = 1;

        //        if (d.DeviceTypeID == 1)
        //        {
        //            Result = AndroidGCMPushNotification.SendNotification(d.Token, TitleAr, TitleEn, DescriptionAr, DescriptionEn);
        //        }
        //        else
        //        {
        //            //var push = new PushBroker();

        //            //push.OnNotificationSent += NotificationSent;
        //            //push.OnChannelException += ChannelException;
        //            //push.OnServiceException += ServiceException;
        //            //push.OnNotificationFailed += NotificationFailed;
        //            //push.OnDeviceSubscriptionExpired += DeviceSubscriptionExpired;
        //            //push.OnDeviceSubscriptionChanged += DeviceSubscriptionChanged;
        //            //push.OnChannelCreated += ChannelCreated;
        //            //push.OnChannelDestroyed += ChannelDestroyed;


        //            //string Certificates = ConfigurationManager.AppSettings["Certificates"];

        //            //var appleCert = File.ReadAllBytes(System.Web.HttpContext.Current.Server.MapPath("~/" + Certificates));

        //            //push.RegisterAppleService(new ApplePushChannelSettings(false, appleCert, System.Configuration.ConfigurationManager.AppSettings["Password"], true));

        //            //push.QueueNotification(new AppleNotification()
        //            //     .ForDeviceToken(d.Token)//the recipient device id
        //            //     .WithAlert(strMessageAr)//the message
        //            //     .WithCategory(category)
        //            //     .WithBadge(1/*d.BadgeNo.Value*/)
        //            //     .WithSound("Default")
        //            //     );
        //            //push.StopAllServices(true);
        //        }

        //        _Context.SaveChanges();
        //    }

        //    return Result;
        //}

        //public static bool PublicPush(int TokenID, string TitleAr, string TitleEn, string DescriptionAr, string DescriptionEn, string ImagePath, int WithPic, long Code, int NotificationTypeID)
        //{
        //    admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

        //    bool Result = false;
        //    //var d = _Context.RegisterDevice.Where(x => x.UserProfileCode == EmployeeID).ToList();
        //    TblRegisterDevice d = _Context.TblRegisterDevices.Where(x => x.Code == TokenID).SingleOrDefault();
        //    string category = string.Format("{0}#${1}", Code, NotificationTypeID);
        //    if (d != null && !string.IsNullOrEmpty(d.Token))
        //    {
        //        if (d.BadgeNo.HasValue && d.BadgeNo.Value > 0)
        //            d.BadgeNo++;
        //        else
        //            d.BadgeNo = 1;

        //        if (d.DeviceTypeID == 1)
        //        {
        //            //Diyafa.Business.aps snd = new Diyafa.Business.aps();
        //            //snd.alertAR = strMessageAr;
        //            //snd.alertEN = strMessageEn;
        //            //snd.category = category;
        //            //snd.badge = 1;//d.BadgeNo.HasValue ? d.BadgeNo.Value : 0;
        //            //snd.sound = "Default";
        //            //PushAndroid(JsonConvert.SerializeObject(snd), d.Token, snd.badge);
        //            if (WithPic == 0)
        //            {
        //                Result = AndroidGCMPushNotification.SendNotification(d.Token, TitleAr, TitleEn, DescriptionAr, DescriptionEn);
        //            }
        //            else if (WithPic == 1)
        //            {
        //                Result = AndroidGCMPushNotification.SendNotificationWithPic(d.Token, TitleAr, TitleEn, DescriptionAr, DescriptionEn, ImagePath);
        //            }
        //        }
        //        else
        //        {
        //            //var push = new PushBroker();

        //            //push.OnNotificationSent += NotificationSent;
        //            //push.OnChannelException += ChannelException;
        //            //push.OnServiceException += ServiceException;
        //            //push.OnNotificationFailed += NotificationFailed;
        //            //push.OnDeviceSubscriptionExpired += DeviceSubscriptionExpired;
        //            //push.OnDeviceSubscriptionChanged += DeviceSubscriptionChanged;
        //            //push.OnChannelCreated += ChannelCreated;
        //            //push.OnChannelDestroyed += ChannelDestroyed;


        //            //string Certificates = System.Configuration.ConfigurationManager.AppSettings["Certificates"];

        //            //var appleCert = File.ReadAllBytes(System.Web.HttpContext.Current.Server.MapPath("~/" + Certificates));

        //            //push.RegisterAppleService(new ApplePushChannelSettings(false, appleCert, System.Configuration.ConfigurationManager.AppSettings["Password"], true));

        //            //push.QueueNotification(new AppleNotification()
        //            //     .ForDeviceToken(d.Token)//the recipient device id
        //            //     .WithAlert(strMessageAr)//the message
        //            //     .WithCategory(category)
        //            //     .WithBadge(1/*d.BadgeNo.Value*/)
        //            //     .WithSound("Default")
        //            //     );
        //            //push.StopAllServices(true);
        //        }

        //        _Context.SaveChanges();
        //    }

        //    return Result;
        //}

    }
}
