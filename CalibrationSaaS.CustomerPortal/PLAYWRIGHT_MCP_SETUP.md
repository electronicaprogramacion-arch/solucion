# Playwright MCP Setup for Customer Portal Visual Testing

## 📋 **Overview**
This guide provides step-by-step instructions for setting up Playwright MCP (Model Context Protocol) for automated visual testing and screenshots of the Customer Portal UI improvements.

## 🔧 **System Requirements**

### **Prerequisites**
- **Node.js 18+**: Download from [nodejs.org](https://nodejs.org/)
- **npm or yarn**: Comes with Node.js
- **.NET 9.0**: Already installed for CalibrationSaaS
- **Git**: For version control of test results

### **Verify Installation**
```bash
node --version    # Should be 18.0.0 or higher
npm --version     # Should be 8.0.0 or higher
dotnet --version  # Should be 9.0.0 or higher
```

## 🚀 **Installation Steps**

### **1. Install Playwright**
```bash
# Navigate to project root
cd /Users/javier/repos/CalibrationSaaS

# Install Playwright globally (optional but recommended)
npm install -g @playwright/test

# Install Playwright in project
npm init -y
npm install --save-dev @playwright/test

# Install browser binaries
npx playwright install
```

### **2. Initialize Playwright Configuration**
```bash
# Create Playwright config (interactive setup)
npx playwright init

# Or create manually (recommended for custom setup)
```

### **3. Create Project Structure**
```bash
# Create test directories
mkdir -p tests/customer-portal
mkdir -p tests/screenshots
mkdir -p tests/visual-regression
```

## 📁 **Configuration Files**

### **1. playwright.config.ts**
```typescript
import { defineConfig, devices } from '@playwright/test';

export default defineConfig({
  testDir: './tests',
  fullyParallel: true,
  forbidOnly: !!process.env.CI,
  retries: process.env.CI ? 2 : 0,
  workers: process.env.CI ? 1 : undefined,
  reporter: [
    ['html'],
    ['json', { outputFile: 'test-results/results.json' }]
  ],
  
  use: {
    baseURL: 'http://localhost:5001',
    trace: 'on-first-retry',
    screenshot: 'only-on-failure',
    video: 'retain-on-failure',
  },

  projects: [
    {
      name: 'chromium',
      use: { ...devices['Desktop Chrome'] },
    },
    {
      name: 'firefox',
      use: { ...devices['Desktop Firefox'] },
    },
    {
      name: 'webkit',
      use: { ...devices['Desktop Safari'] },
    },
    {
      name: 'Mobile Chrome',
      use: { ...devices['Pixel 5'] },
    },
    {
      name: 'Mobile Safari',
      use: { ...devices['iPhone 12'] },
    },
  ],

  webServer: {
    command: 'dotnet run --project src/CalibrationSaaS/CalibrationSaaS.CustomerPortal/CalibrationSaaS.CustomerPortal.csproj --urls "http://localhost:5001"',
    url: 'http://localhost:5001',
    reuseExistingServer: !process.env.CI,
    timeout: 120 * 1000,
  },
});
```

### **2. package.json Scripts**
```json
{
  "name": "calibrationsaas-e2e-tests",
  "version": "1.0.0",
  "scripts": {
    "test": "playwright test",
    "test:ui": "playwright test --ui",
    "test:debug": "playwright test --debug",
    "test:headed": "playwright test --headed",
    "test:visual": "playwright test tests/visual-regression",
    "report": "playwright show-report",
    "screenshots": "playwright test tests/customer-portal/screenshots.spec.ts"
  },
  "devDependencies": {
    "@playwright/test": "^1.40.0"
  }
}
```

## 🧪 **Test Examples**

### **1. Navigation Testing** (`tests/customer-portal/navigation.spec.ts`)
```typescript
import { test, expect } from '@playwright/test';

test.describe('Customer Portal Navigation', () => {
  test.beforeEach(async ({ page }) => {
    // Navigate to customer portal with tenant
    await page.goto('/thermotemp');
  });

  test('should display main navigation menu', async ({ page }) => {
    // Wait for navigation to load
    await expect(page.locator('.rz-sidebar')).toBeVisible();
    
    // Verify menu items
    await expect(page.locator('text=Dashboard')).toBeVisible();
    await expect(page.locator('text=Certificates')).toBeVisible();
    await expect(page.locator('text=Equipment')).toBeVisible();
    await expect(page.locator('text=Work Orders')).toBeVisible();
    await expect(page.locator('text=Reports')).toBeVisible();
  });

  test('should navigate to certificates page', async ({ page }) => {
    await page.click('text=Certificates');
    await expect(page).toHaveURL(/.*\/certificates/);
    await expect(page.locator('h4')).toContainText('Certificates');
  });

  test('should navigate to equipment search', async ({ page }) => {
    await page.click('text=Equipment');
    await page.click('text=Search Equipment');
    await expect(page).toHaveURL(/.*\/equipment\/search/);
    await expect(page.locator('h4')).toContainText('Equipment Search');
  });
});
```

### **2. Visual Regression Testing** (`tests/visual-regression/layouts.spec.ts`)
```typescript
import { test, expect } from '@playwright/test';

test.describe('Visual Regression Tests', () => {
  test('dashboard layout screenshot', async ({ page }) => {
    await page.goto('/thermotemp');
    await page.waitForLoadState('networkidle');
    
    // Take full page screenshot
    await expect(page).toHaveScreenshot('dashboard-full.png');
    
    // Take specific component screenshots
    await expect(page.locator('.dashboard-metrics')).toHaveScreenshot('dashboard-metrics.png');
  });

  test('certificates page layout', async ({ page }) => {
    await page.goto('/thermotemp/certificates');
    await page.waitForLoadState('networkidle');
    
    await expect(page).toHaveScreenshot('certificates-page.png');
    
    // Test different view modes
    await page.click('[data-testid="grid-view-toggle"]');
    await expect(page.locator('.certificates-grid')).toHaveScreenshot('certificates-grid.png');
  });

  test('responsive design - mobile', async ({ page }) => {
    await page.setViewportSize({ width: 375, height: 667 });
    await page.goto('/thermotemp');
    
    await expect(page).toHaveScreenshot('mobile-dashboard.png');
    
    // Test mobile navigation
    await page.click('[data-testid="mobile-menu-toggle"]');
    await expect(page.locator('.rz-sidebar')).toHaveScreenshot('mobile-navigation.png');
  });
});
```

### **3. Screenshot Generation** (`tests/customer-portal/screenshots.spec.ts`)
```typescript
import { test } from '@playwright/test';

test.describe('Customer Portal Screenshots', () => {
  const pages = [
    { name: 'dashboard', url: '/thermotemp', title: 'Dashboard' },
    { name: 'certificates', url: '/thermotemp/certificates', title: 'Certificates' },
    { name: 'equipment-search', url: '/thermotemp/equipment/search', title: 'Equipment Search' },
    { name: 'work-orders', url: '/thermotemp/work-orders', title: 'Work Orders' },
  ];

  for (const pageInfo of pages) {
    test(`screenshot: ${pageInfo.name}`, async ({ page }) => {
      await page.goto(pageInfo.url);
      await page.waitForLoadState('networkidle');
      
      // Take full page screenshot
      await page.screenshot({ 
        path: `tests/screenshots/${pageInfo.name}-full.png`,
        fullPage: true 
      });
      
      // Take viewport screenshot
      await page.screenshot({ 
        path: `tests/screenshots/${pageInfo.name}-viewport.png` 
      });
    });
  }

  test('responsive screenshots', async ({ page }) => {
    const viewports = [
      { name: 'desktop', width: 1920, height: 1080 },
      { name: 'tablet', width: 768, height: 1024 },
      { name: 'mobile', width: 375, height: 667 },
    ];

    for (const viewport of viewports) {
      await page.setViewportSize({ width: viewport.width, height: viewport.height });
      await page.goto('/thermotemp');
      await page.waitForLoadState('networkidle');
      
      await page.screenshot({ 
        path: `tests/screenshots/dashboard-${viewport.name}.png`,
        fullPage: true 
      });
    }
  });
});
```

## 🏃‍♂️ **Running Tests**

### **Basic Commands**
```bash
# Run all tests
npm test

# Run with UI mode (interactive)
npm run test:ui

# Run specific test file
npx playwright test tests/customer-portal/navigation.spec.ts

# Run visual regression tests only
npm run test:visual

# Generate screenshots
npm run screenshots

# Run tests in headed mode (see browser)
npm run test:headed

# Debug specific test
npx playwright test --debug tests/customer-portal/navigation.spec.ts
```

### **CI/CD Integration**
```bash
# Run tests in CI mode
CI=true npx playwright test

# Generate and upload reports
npx playwright test --reporter=html
npx playwright show-report
```

## 📊 **Reports and Results**

### **HTML Report**
```bash
# Generate and view HTML report
npm run report
```

### **Screenshot Comparison**
- Screenshots stored in `test-results/`
- Visual diffs generated automatically
- Baseline images in `tests/screenshots/`

### **Test Results**
- JSON results in `test-results/results.json`
- Trace files for debugging failures
- Video recordings of failed tests

## 🔧 **Integration with Customer Portal**

### **Test Data Setup**
Create `tests/fixtures/test-data.ts`:
```typescript
export const testCustomer = {
  tenantId: 'thermotemp',
  customerId: 1,
  customerName: 'ThermoTemp Solutions'
};

export const mockCertificates = [
  {
    id: 1,
    certificateNumber: 'CERT-2024-001',
    status: 'Issued',
    issueDate: '2024-01-15'
  }
];
```

### **Page Object Model**
Create `tests/pages/certificates-page.ts`:
```typescript
import { Page, Locator } from '@playwright/test';

export class CertificatesPage {
  readonly page: Page;
  readonly searchInput: Locator;
  readonly statusFilter: Locator;
  readonly certificatesGrid: Locator;

  constructor(page: Page) {
    this.page = page;
    this.searchInput = page.locator('[data-testid="certificate-search"]');
    this.statusFilter = page.locator('[data-testid="status-filter"]');
    this.certificatesGrid = page.locator('[data-testid="certificates-grid"]');
  }

  async searchCertificates(query: string) {
    await this.searchInput.fill(query);
    await this.searchInput.press('Enter');
  }

  async filterByStatus(status: string) {
    await this.statusFilter.selectOption(status);
  }
}
```

This setup provides comprehensive visual testing capabilities for the Customer Portal, enabling automated screenshot generation, visual regression testing, and UI validation across multiple devices and browsers.
