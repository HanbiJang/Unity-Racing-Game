
class ActionSelector
{
    IClientAction clientAction;

    public IClientAction CliendAction
    {
        get { return clientAction; }
    }

    public ActionSelector(IClientAction clientAction) 
    {
        this.clientAction = clientAction; 
    }
    public ActionSelector(EPacketID ePacketID)
    {
        clientAction = ClientActionFactory.CreateAction(ePacketID);
    }
    public void Do(byte[] byteData) { clientAction.Do(byteData); }
}
