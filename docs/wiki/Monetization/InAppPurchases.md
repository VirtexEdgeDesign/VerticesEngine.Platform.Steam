# In App Purchases

Below is the function to setup In App Purchases on different platforms. Currently In App Purchases are only supported on mobile platforms.

## Setup
You'll need to add the InAppBilling nuget package.

### Android
For Android you'll also need to add the following permission to your `AndroidManifest.xml` file.
```
<uses-permission android:name="com.android.vending.BILLING" />
```

## Product Keys and SKU

You'll need to add your products in the respective stores, and then have a corresponding key in code. This is often setup as an `enum`.

```csharp
public enum InAppProductTypes
{
    GoAddFree,
}
```

## Initialise Products
In your main game class which inherits from `vxGame`, override the `InitInAppProducts()` method.

```csharp
        protected override void InitInAppProducts()
        {
            vxInAppProductManager.Instance.OnPurchased = delegate (vxInAppProduct product) {
                vxNotificationManager.Show(product.Name + " Purchased", Color.CornflowerBlue);
            };
            vxInAppProductManager.Instance.OnPurchaseConsumed = delegate (vxInAppProduct product) {
                vxNotificationManager.Show(product.Name + " Purchased Consumed ", Color.CornflowerBlue);
            };

            vxInAppProductManager.Instance.AddProduct(InAppProductTypes.GoAddFree, new vxInAppProduct("Go Ad Free", vxInAppProductType.NonConsumable, new vxPlatformString("", "game_sku_for_remove_ads")));
            vxInAppProductManager.Instance.RestorePurchases();
        }
```

## Check If Purchased
All purchases are restored on launch, if you'd like to check if an item has been previously purchased you can do so by calling:

```csharp
            if (vxInAppProductManager.Instance.GetProduct(InAppProductTypes.GoAddFree).IsPurchased == false)
            {
                MenuEntries.Add(upgradeMenuEntry);
            }
```