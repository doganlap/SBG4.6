'use client'

import { Button } from '@/components/ui/button'
import { Badge } from '@/components/ui/badge'
import { 
  ArrowRight, CheckCircle, Play, Star, 
  Zap, Shield, Globe, Layers
} from 'lucide-react'
import Link from 'next/link'
import { ModulesShowcase } from '@/components/sections/modules-showcase'
import { PricingSection } from '@/components/sections/pricing-section'
import { TestimonialsSection } from '@/components/sections/testimonials-section'
import { FeaturesSection } from '@/components/sections/features-section'
import { CTASection } from '@/components/sections/cta-section'

const stats = [
  { value: '22+', label: 'Modules' },
  { value: '10k+', label: 'Customers' },
  { value: '99.9%', label: 'Uptime' },
  { value: '24/7', label: 'Support' },
]

const trustedBy = [
  'Acme Corp', 'Global Industries', 'TechStart', 'MediCare', 'EduWorld'
]

export default function HomePage() {
  return (
    <main>
      {/* Hero Section */}
      <section className="relative min-h-screen flex items-center justify-center overflow-hidden">
        {/* Background */}
        <div className="absolute inset-0 gradient-bg" />
        <div className="absolute inset-0 grid-pattern opacity-30" />
        
        {/* Floating orbs */}
        <div className="absolute top-1/4 left-1/4 w-96 h-96 bg-primary/20 rounded-full blur-3xl animate-pulse" />
        <div className="absolute bottom-1/4 right-1/4 w-80 h-80 bg-purple-500/20 rounded-full blur-3xl animate-pulse delay-1000" />

        <div className="relative z-10 max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 text-center">
          {/* Badge */}
          <Badge variant="outline" className="mb-6 px-4 py-2 text-sm backdrop-blur-sm">
            <Zap className="w-4 h-4 mr-2 text-yellow-500" />
            Now with 22 fully customizable modules
          </Badge>

          {/* Headline */}
          <h1 className="text-5xl md:text-7xl font-bold tracking-tight mb-6">
            <span className="gradient-text">Enterprise ERP</span>
            <br />
            <span>Made Simple</span>
          </h1>

          {/* Subheadline */}
          <p className="text-xl md:text-2xl text-muted-foreground max-w-3xl mx-auto mb-10">
            Complete business management with Accounting, CRM, HR, Manufacturing, 
            Healthcare, Education, and 16 more modules. All in one platform.
          </p>

          {/* CTA Buttons */}
          <div className="flex flex-col sm:flex-row gap-4 justify-center mb-16">
            <Button size="lg" className="text-lg px-8 py-6 glow">
              Start Free Trial
              <ArrowRight className="ml-2 h-5 w-5" />
            </Button>
            <Button size="lg" variant="outline" className="text-lg px-8 py-6">
              <Play className="mr-2 h-5 w-5" />
              Watch Demo
            </Button>
          </div>

          {/* Stats */}
          <div className="grid grid-cols-2 md:grid-cols-4 gap-8 max-w-3xl mx-auto mb-16">
            {stats.map((stat) => (
              <div key={stat.label} className="text-center">
                <div className="text-4xl font-bold gradient-text">{stat.value}</div>
                <div className="text-muted-foreground">{stat.label}</div>
              </div>
            ))}
          </div>

          {/* Trusted By */}
          <div className="text-muted-foreground">
            <p className="text-sm mb-4">Trusted by leading organizations</p>
            <div className="flex flex-wrap justify-center gap-8">
              {trustedBy.map((company) => (
                <span key={company} className="text-lg font-semibold opacity-50 hover:opacity-100 transition-opacity">
                  {company}
                </span>
              ))}
            </div>
          </div>
        </div>

        {/* Scroll indicator */}
        <div className="absolute bottom-10 left-1/2 transform -translate-x-1/2 animate-bounce">
          <div className="w-6 h-10 border-2 border-muted-foreground/30 rounded-full flex justify-center pt-2">
            <div className="w-1 h-2 bg-muted-foreground/50 rounded-full" />
          </div>
        </div>
      </section>

      {/* Features Section */}
      <FeaturesSection />

      {/* Modules Showcase */}
      <ModulesShowcase />

      {/* Pricing Section */}
      <PricingSection />

      {/* Testimonials */}
      <TestimonialsSection />

      {/* CTA Section */}
      <CTASection />
    </main>
  )
}
