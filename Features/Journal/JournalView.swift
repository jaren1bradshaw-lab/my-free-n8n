import SwiftUI

struct JournalView: View {
    var body: some View {
        NavigationStack {
            Text("Journal")
                .font(.title.bold())
                .frame(maxWidth: .infinity, maxHeight: .infinity)
                .background(ProKopeTheme.backgroundColor)
                .foregroundStyle(.white)
                .navigationTitle("Journal")
        }
    }
}

#Preview {
    JournalView()
}
