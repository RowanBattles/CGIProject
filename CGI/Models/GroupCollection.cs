namespace CGI.Models;

public class GroupCollection
{
    public List<Group> groups { get; }

    public GroupCollection()
    {
        groups = new List<Group>();
    }
    
    public GroupCollection(List<Group> groups)
    {
        this.groups = groups;
    }

    public void AddGroup(Group group)
    {
        groups.Add(group);
    }
}