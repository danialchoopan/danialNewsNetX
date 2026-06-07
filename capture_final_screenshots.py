import asyncio
from playwright.async_api import async_playwright
import time
import os
import subprocess

async def capture_final_screenshots():
    # Start the .NET app
    env = os.environ.copy()
    env["ASPNETCORE_ENVIRONMENT"] = "Development"

    # Try starting the server on a known port
    process = subprocess.Popen(
        ["dotnet", "run", "--project", "src/WebUI/danialNewsNetX.WebUI.csproj", "--urls", "http://localhost:5037"],
        stdout=subprocess.PIPE,
        stderr=subprocess.PIPE,
        env=env
    )

    print("Starting .NET server for final screenshots...")
    time.sleep(30)

    async with async_playwright() as p:
        browser = await p.chromium.launch()
        page = await browser.new_page()
        port = 5037

        try:
            # 1. New Improved Landing Page
            print(f"Navigating to http://localhost:{port}")
            await page.goto(f"http://localhost:{port}")
            await page.wait_for_timeout(3000)
            await page.screenshot(path="docs/images/home_fa.png", full_page=True)
            print("Captured Improved Landing Page")

            # 2. Normal User Feed
            print(f"Navigating to http://localhost:{port}/feed")
            await page.goto(f"http://localhost:{port}/feed")
            await page.wait_for_timeout(3000)
            await page.screenshot(path="docs/images/user_feed_fa.png", full_page=True)
            print("Captured User Feed")

            # 3. Super Admin Dashboard (already verified, but recapture for consistency)
            print(f"Navigating to http://localhost:{port}/admin/super")
            await page.goto(f"http://localhost:{port}/admin/super")
            await page.wait_for_timeout(3000)
            await page.screenshot(path="docs/images/admin_dashboard_fa.png", full_page=True)
            print("Captured Admin Dashboard")

        except Exception as e:
            print(f"Error during navigation: {e}")

        await browser.close()

    process.terminate()

if __name__ == "__main__":
    asyncio.run(capture_final_screenshots())
