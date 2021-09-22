using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommunityBox.Common.Core.SystemChat
{
    public static class SystemChatMessageTemplates
    {
        public static string GetOnSubMessageTemplate(string userId, long auctionId) =>
            $"Пользователь {userId} подписался на Ваш аукцион - {auctionId}";

        public static string GetOnUnsubMessageTemplate(string userId, long auctionId) =>
            $"Пользователь {userId} отписался на Вашего аукциона - {auctionId}";

        public static string GetSetBetMessageTemplate(string userId, long auctionId, decimal value) =>
            $"Пользователь {userId} сделал ставку для Вашего аукциона - {auctionId} на сумму - {value}";

        public static string GetRemoveBetMessageTemplate(string userId, long auctionId) =>
            $"Пользователь {userId} удалил ставку для Вашего аукциона - {auctionId}";

    }
}