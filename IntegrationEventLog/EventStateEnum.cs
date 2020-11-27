using System;
using System.Collections.Generic;
using System.Text;

namespace IntegrationEventLog
{
    public static class EventStateEnum
    {
        public static int NotPublished = 0;
        public static int InProgress = 1;
        public static int Published = 2;
        public static int PublishedFailed = 3;
    }
}
