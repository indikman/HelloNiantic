namespace Niantic.ARDK.AR.WayspotAnchors
{
  // Configurations are for internal testing and debugging.
  internal interface _IWayspotAnchorsDebugConfiguration
  {
    /// Override option internal testing.
    bool CloudProcessingForced { get; set; }

    /// Override option internal testing.
    bool ClientProcessingForced { get; set; }

    /// The endpoint for VPS config API requests.
    string ConfigURL { get; set; }

    /// The endpoint for VPS health API requests.
    string HealthURL { get; set; }

    /// The endpoint for VPS localization API requests.
    string LocalizationURL { get; set; }

    /// The endpoint for VPS graph sync API requests.
    string GraphSyncURL { get; set; }

    /// The endpoint for VPS anchor creation API requests.
    string WayspotAnchorCreateURL { get; set; }

    /// The endpoint for VPS anchor resolution API requests.
    string WayspotAnchorResolveURL { get; set; }

    /// The endpoint for VPS node registration API requests.
    string RegisterNodeURL { get; set; }

    /// The endpoint for VPS node lookup API requests.
    string LookUpNodeURL { get; set; }
  }
}
