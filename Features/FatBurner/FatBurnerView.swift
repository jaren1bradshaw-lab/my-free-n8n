import SwiftUI

struct FatBurnerView: View {
    var body: some View {
        NavigationStack {
            Text("Fat Burner")
                .font(.title.bold())
                .frame(maxWidth: .infinity, maxHeight: .infinity)
                .background(ProKopeTheme.backgroundColor)
                .foregroundStyle(.white)
                .navigationTitle("Fat Burner")
        }
    }
}

#Preview {
    FatBurnerView()
}
