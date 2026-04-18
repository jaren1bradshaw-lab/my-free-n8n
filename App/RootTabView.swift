import SwiftUI

struct RootTabView: View {
    var body: some View {
        TabView {
            FatBurnerView()
                .tabItem {
                    Label("Fat Burner", systemImage: "flame.fill")
                }

            WorkoutsView()
                .tabItem {
                    Label("Workouts", systemImage: "figure.strengthtraining.traditional")
                }

            DietView()
                .tabItem {
                    Label("Diet", systemImage: "leaf.fill")
                }

            GoalsView()
                .tabItem {
                    Label("Goals", systemImage: "target")
                }

            JournalView()
                .tabItem {
                    Label("Journal", systemImage: "book.closed.fill")
                }
        }
    }
}

#Preview {
    RootTabView()
}
