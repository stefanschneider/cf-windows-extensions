﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Channels;

namespace Uhuru.Utilities
{
    public class MonitoringServer
    {
        private int serverPort;
        WebServiceHost host;
        private static MonitoringServer instance = null;

        public delegate string HealthzRequestedHandler(object sender, EventArgs e);
        public event HealthzRequestedHandler HealthzRequested;

        public delegate string VarzRequestedHandler(object sender, EventArgs e);
        public event VarzRequestedHandler VarzRequested;

        public MonitoringServer(int port)
        {
            serverPort = port;
            if (instance == null)
            {
                instance = this;
            }
        }

        public void Start()
        {
            Uri baseAddress = new Uri("http://localhost:" + serverPort);
            host = new WebServiceHost(typeof(MonitoringService), baseAddress);
            host.Open();
        }

        public void Stop()
        {
            host.Close();
        }

        public string TriggerHealthz(object sender, EventArgs e)
        {
            string message = string.Empty;
            if (HealthzRequested != null)
            {
                 message = HealthzRequested(sender, e);
            }
            return message;
        }

        public string TriggerVarz(object sender, EventArgs e)
        {
            string message = string.Empty;
            if (VarzRequested != null)
            {
                message = VarzRequested(sender, e);
            }
            return message;
        }

        public static MonitoringServer Instance
        {
            get
            {
                return instance;
            }
        }        
    }

    [ServiceContract]
    interface IMonitoringService
    {
        [WebGet(UriTemplate = "/heathz")]
        Message GetHealthz();

        [WebGet(UriTemplate = "/varz")]
        Message GetVarz();
    }

    public class MonitoringService : IMonitoringService
    {
        public Message GetHealthz()
        {
            string message = MonitoringServer.Instance.TriggerHealthz(this, EventArgs.Empty);
            try
            {
                return CreateTextresponse(message, "text/plaintext");
            }
            catch (Exception ex)
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                WebOperationContext.Current.OutgoingResponse.StatusDescription = ex.ToString();
                return null;
            }
        }

        public Message GetVarz()
        {
            string message = MonitoringServer.Instance.TriggerVarz(this, EventArgs.Empty);
            try
            {
                return CreateTextresponse(message, "application/json");
            }
            catch (Exception ex)
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                WebOperationContext.Current.OutgoingResponse.StatusDescription = ex.ToString();
                return null;
            }
        }

        private Message CreateTextresponse(string message, string contentType)
        {
            return WebOperationContext.Current.CreateTextResponse(message, contentType);
        }
    }
}
