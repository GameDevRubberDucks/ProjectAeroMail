using UnityEngine.Events;

//--- Helper Event Classes ---//
public class Delivery_ZoneChangeEvent : UnityEvent<Delivery_Zone> {}
public class Delivery_TargetChangeEvent : UnityEvent<Delivery_End> { }
