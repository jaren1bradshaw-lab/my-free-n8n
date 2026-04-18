import SwiftUI

@main
struct ProKopeWatchApp: App {
    var body: some Scene {
        WindowGroup {
            WatchRootView()
                .preferredColorScheme(.dark)
        }
    }
}
