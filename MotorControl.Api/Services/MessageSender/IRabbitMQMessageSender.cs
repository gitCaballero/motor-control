﻿using MotorControl.Api.Models;

namespace RentalMotor.Api.Services.Network.MessageSender
{
    public interface IRabbitMQMessageSender
    {
        void SendMessage(IEnumerable<BaseMessage> messages, string queueName, string typeObejct);
    }
}
