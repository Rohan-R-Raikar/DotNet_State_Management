namespace SampleNo.Models
{
    public class DashboardViewModel
    {
        public string CurrentUserId { get; set; }

        public List<PostViewModel> FollowedUserPost { get; set; }
        public List<StoryViewModel> FollowedUserStory { get; set; }
        public List<UserSearchViewModel?> SearchedUsers { get; set; }
    }
}
