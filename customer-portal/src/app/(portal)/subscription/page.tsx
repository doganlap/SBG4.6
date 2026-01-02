'use client'

import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Badge } from '@/components/ui/badge'
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs'
import { Check, CreditCard, Download, Package } from 'lucide-react'

const plans = [
  {
    name: 'Starter',
    price: '$499',
    period: '/month',
    description: 'Perfect for small businesses',
    features: [
      '5 users included',
      '10 GB storage',
      '4 core modules',
      'Email support',
      'Daily backups',
    ],
    current: false,
  },
  {
    name: 'Professional',
    price: '$1,500',
    period: '/month',
    description: 'For growing organizations',
    features: [
      '50 users included',
      '50 GB storage',
      '8 modules included',
      'Priority support',
      'Hourly backups',
      'API access',
      'Custom reports',
    ],
    current: true,
    popular: true,
  },
  {
    name: 'Enterprise',
    price: '$4,500',
    period: '/month',
    description: 'For large organizations',
    features: [
      'Unlimited users',
      '500 GB storage',
      'All 22 modules',
      'Dedicated support',
      'Real-time backups',
      'Full API access',
      'Custom development',
      'SLA guarantee',
      'Dedicated instance',
    ],
    current: false,
  },
]

const invoices = [
  { id: 'INV-2024-012', date: 'Dec 15, 2024', amount: '$1,500.00', status: 'paid' },
  { id: 'INV-2024-011', date: 'Nov 15, 2024', amount: '$1,500.00', status: 'paid' },
  { id: 'INV-2024-010', date: 'Oct 15, 2024', amount: '$1,500.00', status: 'paid' },
  { id: 'INV-2024-009', date: 'Sep 15, 2024', amount: '$1,500.00', status: 'paid' },
  { id: 'INV-2024-008', date: 'Aug 15, 2024', amount: '$1,500.00', status: 'paid' },
]

export default function SubscriptionPage() {
  return (
    <div className="space-y-6">
      {/* Header */}
      <div>
        <h1 className="text-3xl font-bold tracking-tight">Subscription</h1>
        <p className="text-muted-foreground">
          Manage your subscription and billing
        </p>
      </div>

      <Tabs defaultValue="plans" className="space-y-6">
        <TabsList>
          <TabsTrigger value="plans">Plans</TabsTrigger>
          <TabsTrigger value="billing">Billing</TabsTrigger>
          <TabsTrigger value="invoices">Invoices</TabsTrigger>
        </TabsList>

        {/* Plans Tab */}
        <TabsContent value="plans" className="space-y-6">
          <div className="grid gap-6 md:grid-cols-3">
            {plans.map((plan) => (
              <Card 
                key={plan.name} 
                className={plan.current ? 'border-primary ring-2 ring-primary' : ''}
              >
                <CardHeader>
                  <div className="flex items-center justify-between">
                    <CardTitle>{plan.name}</CardTitle>
                    {plan.current && <Badge className="bg-primary">Current</Badge>}
                    {plan.popular && !plan.current && (
                      <Badge variant="secondary">Popular</Badge>
                    )}
                  </div>
                  <CardDescription>{plan.description}</CardDescription>
                </CardHeader>
                <CardContent className="space-y-6">
                  <div>
                    <span className="text-3xl font-bold">{plan.price}</span>
                    <span className="text-muted-foreground">{plan.period}</span>
                  </div>
                  <ul className="space-y-2">
                    {plan.features.map((feature) => (
                      <li key={feature} className="flex items-center gap-2 text-sm">
                        <Check className="h-4 w-4 text-primary" />
                        {feature}
                      </li>
                    ))}
                  </ul>
                  {plan.current ? (
                    <Button variant="outline" className="w-full" disabled>
                      Current Plan
                    </Button>
                  ) : (
                    <Button className="w-full">
                      {plan.name === 'Starter' ? 'Downgrade' : 'Upgrade'}
                    </Button>
                  )}
                </CardContent>
              </Card>
            ))}
          </div>
        </TabsContent>

        {/* Billing Tab */}
        <TabsContent value="billing" className="space-y-6">
          <Card>
            <CardHeader>
              <CardTitle>Payment Method</CardTitle>
              <CardDescription>Manage your payment details</CardDescription>
            </CardHeader>
            <CardContent className="space-y-4">
              <div className="flex items-center justify-between rounded-lg border p-4">
                <div className="flex items-center gap-4">
                  <div className="flex h-12 w-12 items-center justify-center rounded-lg bg-muted">
                    <CreditCard className="h-6 w-6" />
                  </div>
                  <div>
                    <div className="font-medium">Visa ending in 4242</div>
                    <div className="text-sm text-muted-foreground">Expires 12/2026</div>
                  </div>
                </div>
                <Badge>Default</Badge>
              </div>
              <Button variant="outline">
                <CreditCard className="mr-2 h-4 w-4" />
                Add Payment Method
              </Button>
            </CardContent>
          </Card>

          <Card>
            <CardHeader>
              <CardTitle>Billing Information</CardTitle>
              <CardDescription>Your billing address and details</CardDescription>
            </CardHeader>
            <CardContent className="space-y-4">
              <div className="grid gap-4 md:grid-cols-2">
                <div>
                  <div className="text-sm font-medium">Company Name</div>
                  <div className="text-sm text-muted-foreground">Acme Corporation</div>
                </div>
                <div>
                  <div className="text-sm font-medium">Email</div>
                  <div className="text-sm text-muted-foreground">billing@acme.com</div>
                </div>
                <div>
                  <div className="text-sm font-medium">Address</div>
                  <div className="text-sm text-muted-foreground">
                    123 Business Street, Suite 100<br />
                    San Francisco, CA 94102
                  </div>
                </div>
                <div>
                  <div className="text-sm font-medium">Tax ID</div>
                  <div className="text-sm text-muted-foreground">US-123456789</div>
                </div>
              </div>
              <Button variant="outline">Update Billing Info</Button>
            </CardContent>
          </Card>
        </TabsContent>

        {/* Invoices Tab */}
        <TabsContent value="invoices" className="space-y-6">
          <Card>
            <CardHeader>
              <CardTitle>Invoice History</CardTitle>
              <CardDescription>Download and view past invoices</CardDescription>
            </CardHeader>
            <CardContent>
              <div className="space-y-4">
                {invoices.map((invoice) => (
                  <div 
                    key={invoice.id} 
                    className="flex items-center justify-between rounded-lg border p-4"
                  >
                    <div className="flex items-center gap-4">
                      <div className="flex h-10 w-10 items-center justify-center rounded-lg bg-muted">
                        <Package className="h-5 w-5" />
                      </div>
                      <div>
                        <div className="font-medium">{invoice.id}</div>
                        <div className="text-sm text-muted-foreground">{invoice.date}</div>
                      </div>
                    </div>
                    <div className="flex items-center gap-4">
                      <div className="text-right">
                        <div className="font-medium">{invoice.amount}</div>
                        <Badge variant="secondary" className="text-xs">
                          {invoice.status}
                        </Badge>
                      </div>
                      <Button variant="ghost" size="icon">
                        <Download className="h-4 w-4" />
                      </Button>
                    </div>
                  </div>
                ))}
              </div>
            </CardContent>
          </Card>
        </TabsContent>
      </Tabs>
    </div>
  )
}
