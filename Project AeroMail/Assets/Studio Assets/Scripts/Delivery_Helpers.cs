﻿using UnityEngine.Events;

//--- Helper Event Classes ---//
public class Delivery_TargetChangeEvent : UnityEvent<Delivery_End> { }
public class Delivery_TargetListChangeEvent : UnityEvent<int> { }
public class Delivery_CounterChangeEvent : UnityEvent<int, int> { }