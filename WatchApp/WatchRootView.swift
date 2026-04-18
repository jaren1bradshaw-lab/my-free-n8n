import SwiftUI

struct WatchRootView: View {
    var body: some View {
        TabView {
            Text("Track")
            Text("Workouts")
            Text("Goals")
        }
        .tabViewStyle(.page)
    }
}

#Preview {
    WatchRootView()
}
