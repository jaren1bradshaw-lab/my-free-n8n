import SwiftUI

@main
struct ProKopeApp: App {
    var body: some Scene {
        WindowGroup {
            RootTabView()
                .preferredColorScheme(.dark)
                .tint(.mint)
        }
    }
}
