'use client'

import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card'
import { Badge } from '@/components/ui/badge'
import { Button } from '@/components/ui/button'
import { 
  Calculator, Users, ShoppingCart, Truck, Package, Factory,
  ClipboardList, Headphones, Globe, CreditCard, Stethoscope,
  GraduationCap, Building2, Wheat, Heart, Hotel,
  BarChart3, Wrench, Shield, FileText, Workflow, Puzzle
} from 'lucide-react'
import Link from 'next/link'

const modules = [
  { id: 1, name: 'Accounting', description: 'Financial management, ledgers, reports', icon: Calculator, category: 'Core' },
  { id: 2, name: 'CRM', description: 'Customer relationship management', icon: Users, category: 'Core' },
  { id: 3, name: 'Selling', description: 'Sales orders, quotations, invoices', icon: ShoppingCart, category: 'Core' },
  { id: 4, name: 'Buying', description: 'Purchase orders, supplier management', icon: Truck, category: 'Core' },
  { id: 5, name: 'Stock', description: 'Inventory management, warehouses', icon: Package, category: 'Core' },
  { id: 6, name: 'Manufacturing', description: 'Production planning, BOM, work orders', icon: Factory, category: 'Core' },
  { id: 7, name: 'Projects', description: 'Project management, tasks, timesheets', icon: ClipboardList, category: 'Core' },
  { id: 8, name: 'Support', description: 'Helpdesk, tickets, SLA', icon: Headphones, category: 'Core' },
  { id: 9, name: 'Website', description: 'Web pages, blog, portal', icon: Globe, category: 'Core' },
  { id: 10, name: 'E-commerce', description: 'Online store, shopping cart', icon: CreditCard, category: 'Core' },
  { id: 11, name: 'POS', description: 'Point of sale system', icon: CreditCard, category: 'Core' },
  { id: 12, name: 'Assets', description: 'Fixed assets, depreciation', icon: Building2, category: 'Core' },
  { id: 13, name: 'HRMS', description: 'Employee management, payroll', icon: Users, category: 'Standalone' },
  { id: 14, name: 'Education', description: 'Schools, courses, students', icon: GraduationCap, category: 'Domain' },
  { id: 15, name: 'Healthcare', description: 'Patients, appointments, lab tests', icon: Stethoscope, category: 'Domain' },
  { id: 16, name: 'Payments', description: 'Payment gateway integrations', icon: CreditCard, category: 'Standalone' },
  { id: 17, name: 'LMS', description: 'Learning Management System', icon: GraduationCap, category: 'Standalone' },
  { id: 18, name: 'Helpdesk', description: 'Customer support portal', icon: Headphones, category: 'Standalone' },
  { id: 19, name: 'Wiki', description: 'Knowledge base & docs', icon: FileText, category: 'Standalone' },
  { id: 20, name: 'Insights', description: 'Business intelligence & analytics', icon: BarChart3, category: 'Standalone' },
  { id: 21, name: 'Builder', description: 'Visual website/app builder', icon: Puzzle, category: 'Standalone' },
  { id: 22, name: 'CRM App', description: 'Standalone CRM application', icon: Users, category: 'Standalone' },
]

const categories = ['All', 'Core', 'Domain', 'Standalone']

export function ModulesShowcase() {
  return (
    <section id="modules" className="py-24 bg-muted/30">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        {/* Section Header */}
        <div className="text-center mb-16">
          <Badge variant="outline" className="mb-4">22 Modules</Badge>
          <h2 className="text-4xl md:text-5xl font-bold mb-6">
            Everything You Need
          </h2>
          <p className="text-xl text-muted-foreground max-w-2xl mx-auto">
            From accounting to healthcare, our comprehensive suite covers 
            every aspect of your business operations.
          </p>
        </div>

        {/* Category Filter */}
        <div className="flex flex-wrap justify-center gap-2 mb-12">
          {categories.map((category) => (
            <Button 
              key={category} 
              variant={category === 'All' ? 'default' : 'outline'}
              size="sm"
            >
              {category}
            </Button>
          ))}
        </div>

        {/* Modules Grid */}
        <div className="grid gap-6 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4">
          {modules.map((module) => (
            <Card 
              key={module.id} 
              className="group hover:border-primary/50 hover:shadow-lg transition-all duration-300 cursor-pointer"
            >
              <CardHeader className="pb-3">
                <div className="flex items-start justify-between">
                  <div className="flex h-12 w-12 items-center justify-center rounded-xl bg-primary/10 group-hover:bg-primary/20 transition-colors">
                    <module.icon className="h-6 w-6 text-primary" />
                  </div>
                  <Badge variant="secondary" className="text-xs">
                    {module.category}
                  </Badge>
                </div>
                <CardTitle className="text-lg mt-4">{module.name}</CardTitle>
              </CardHeader>
              <CardContent>
                <CardDescription className="line-clamp-2">
                  {module.description}
                </CardDescription>
                <Button variant="link" className="mt-4 p-0 h-auto text-primary">
                  Learn more →
                </Button>
              </CardContent>
            </Card>
          ))}
        </div>

        {/* CTA */}
        <div className="text-center mt-16">
          <Button size="lg" asChild>
            <Link href="/modules">
              View All Module Details
            </Link>
          </Button>
        </div>
      </div>
    </section>
  )
}
