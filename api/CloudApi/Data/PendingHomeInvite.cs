using CommonData.Model.Entity;

namespace CloudApi.Data;

public class PendingHomeInvite
{

    public Home HomeToJoin { get; set; }

    public Person PersonThatInvited { get; set; }

    public Person PersonToJoin { get; set; }
    
}