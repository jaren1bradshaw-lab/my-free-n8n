import SwiftUI

struct GoalsView: View {
    var body: some View {
        NavigationStack {
            Text("Goals")
                .font(.title.bold())
                .frame(maxWidth: .infinity, maxHeight: .infinity)
                .background(ProKopeTheme.backgroundColor)
                .foregroundStyle(.white)
                .navigationTitle("Goals")
        }
    }
}

#Preview {
    GoalsView()
}
