import SwiftUI

struct WorkoutsView: View {
    var body: some View {
        NavigationStack {
            Text("Workouts")
                .font(.title.bold())
                .frame(maxWidth: .infinity, maxHeight: .infinity)
                .background(ProKopeTheme.backgroundColor)
                .foregroundStyle(.white)
                .navigationTitle("Workouts")
        }
    }
}

#Preview {
    WorkoutsView()
}
