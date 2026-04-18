import SwiftUI

struct DietView: View {
    var body: some View {
        NavigationStack {
            Text("Diet")
                .font(.title.bold())
                .frame(maxWidth: .infinity, maxHeight: .infinity)
                .background(ProKopeTheme.backgroundColor)
                .foregroundStyle(.white)
                .navigationTitle("Diet")
        }
    }
}

#Preview {
    DietView()
}
