using Newtonsoft.Json;

namespace StringeeCallWeb.DTOs
{
    public class CallLogResponse
    {
        public class Call
        {
            [JsonProperty("answer_time")]
            public int AnswerTime { get; set; }

            [JsonProperty("stop_time")]
            public long StopTime { get; set; }

            [JsonProperty("first_answer_time")]
            public int FirstAnswerTime { get; set; }

            [JsonProperty("to_alias")]
            public string ToAlias { get; set; }

            [JsonProperty("answer_duration")]
            public int AnswerDuration { get; set; }

            [JsonProperty("from_internal")]
            public int FromInternal { get; set; }

            [JsonProperty("number_tts_character")]
            public int NumberTtsCharacter { get; set; }

            [JsonProperty("project_id")]
            public string ProjectId { get; set; }

            [JsonProperty("from_number")]
            public string FromNumber { get; set; }

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("day")]
            public string Day { get; set; }

            [JsonProperty("from_user_id")]
            public string FromUserId { get; set; }

            [JsonProperty("to_internal")]
            public int ToInternal { get; set; }

            [JsonProperty("to_number")]
            public string ToNumber { get; set; }

            [JsonProperty("amount")]
            public string Amount { get; set; }

            [JsonProperty("object_type")]
            public string ObjectType { get; set; }

            [JsonProperty("created")]
            public int Created { get; set; }

            [JsonProperty("video_call")]
            public int VideoCall { get; set; }

            [JsonProperty("recorded")]
            public int Recorded { get; set; }

            [JsonProperty("start_time")]
            public long StartTime { get; set; }

            [JsonProperty("account_id")]
            public int AccountId { get; set; }

            [JsonProperty("from_alias")]
            public string FromAlias { get; set; }

            [JsonProperty("answer_duration_minutes")]
            public int AnswerDurationMinutes { get; set; }

            [JsonProperty("start_time_datetime")]
            public string StartTimeDatetime { get; set; }

            [JsonProperty("answer_time_datetime")]
            public string AnswerTimeDatetime { get; set; }

            [JsonProperty("stop_time_datetime")]
            public string StopTimeDatetime { get; set; }

            [JsonProperty("created_datetime")]
            public string CreatedDatetime { get; set; }

            [JsonProperty("project_name")]
            public string ProjectName { get; set; }

            [JsonProperty("uuid")]
            public object Uuid { get; set; }

            [JsonProperty("record_path")]
            public object RecordPath { get; set; }

            [JsonProperty("participants")]
            public object Participants { get; set; }
        }

        public class Data
        {
            [JsonProperty("calls")]
            public List<Call> Calls { get; set; }

            [JsonProperty("currentPage")]
            public int CurrentPage { get; set; }

            [JsonProperty("limit")]
            public int Limit { get; set; }

            [JsonProperty("totalPages")]
            public int TotalPages { get; set; }

            [JsonProperty("totalCalls")]
            public string TotalCalls { get; set; }

            [JsonProperty("totalAnswerDurationMinutes")]
            public int TotalAnswerDurationMinutes { get; set; }
        }

        public class Root
        {
            [JsonProperty("r")]
            public int R { get; set; }

            [JsonProperty("message")]
            public string Message { get; set; }

            [JsonProperty("data")]
            public Data Data { get; set; }
        }
    }
}
